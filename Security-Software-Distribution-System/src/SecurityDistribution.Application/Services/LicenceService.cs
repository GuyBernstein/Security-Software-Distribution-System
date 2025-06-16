using Security_Software_Distribution_System.SecurityDistribution.Application.Interfaces;
using Security_Software_Distribution_System.SecurityDistribution.Domain.Entities;
using Security_Software_Distribution_System.SecurityDistribution.Domain.Enums;

namespace Security_Software_Distribution_System.SecurityDistribution.Application.Services
{
    /// <summary>
    /// Service for managing software licenses
    /// </summary>
    public class LicenseService
    {
        private readonly ILicenseRepository _licenseRepository;
        private readonly ISecurityEventRepository _securityEventRepository;
        
        public LicenseService(
            ILicenseRepository licenseRepository, 
            ISecurityEventRepository securityEventRepository)
        {
            _licenseRepository = licenseRepository ?? throw new ArgumentNullException(nameof(licenseRepository));
            _securityEventRepository = securityEventRepository ?? throw new ArgumentNullException(nameof(securityEventRepository));
        }
        
        /// <summary>
        /// Creates a new license for a customer
        /// </summary>
        public async Task<CreateLicenseResult> CreateLicenseAsync(
            string productName, 
            string customerEmail, 
            int validityDays, 
            int maxActivations)
        {
            try
            {
                // Create the license using domain logic
                var license = new License(productName, customerEmail, validityDays, maxActivations);
                
                // Persist it
                await _licenseRepository.AddAsync(license);
                
                return new CreateLicenseResult
                {
                    Success = true,
                    LicenseKey = license.LicenseKey,
                    ExpirationDate = license.ExpirationDate
                };
            }
            catch (ArgumentException ex)
            {
                return new CreateLicenseResult
                {
                    Success = false,
                    ErrorMessage = ex.Message
                };
            }
            catch (Exception ex)
            {
                // Log this exception in a real application
                return new CreateLicenseResult
                {
                    Success = false,
                    ErrorMessage = "An error occurred while creating the license"
                };
            }
        }
        
        /// <summary>
        /// Validates if a license key is valid and active
        /// </summary>
        public async Task<LicenseValidationResult> ValidateLicenseAsync(string licenseKey)
        {
            var license = await _licenseRepository.GetByKeyAsync(licenseKey);
            
            if (license == null)
            {
                // Log security event for invalid license attempt
                // await _securityEventRepository.AddAsync(new SecurityEvent(
                //     SecurityEventType.InvalidLicenseAttempt,
                //     licenseKey,
                //     "Attempted to validate non-existent license",
                //     null
                // ));
                
                return new LicenseValidationResult
                {
                    IsValid = false,
                    Reason = "License key not found"
                };
            }
            
            if (!license.IsActive)
            {
                return new LicenseValidationResult
                {
                    IsValid = false,
                    Reason = "License has been revoked"
                };
            }
            
            if (license.IsExpired())
            {
                // Log security event for expired license use
                // await _securityEventRepository.AddAsync(new SecurityEvent(
                //     SecurityEventType.ExpiredLicenseUse,
                //     licenseKey,
                //     $"Attempted to use expired license (expired on {license.ExpirationDate:yyyy-MM-dd})",
                //     null
                // ));
                
                return new LicenseValidationResult
                {
                    IsValid = false,
                    Reason = "License has expired"
                };
            }
            
            return new LicenseValidationResult
            {
                IsValid = true,
                License = license
            };
        }
        
        /// <summary>
        /// Gets all licenses for a customer
        /// </summary>
        public async Task<IEnumerable<LicenseDto>> GetCustomerLicensesAsync(string customerEmail)
        {
            var licenses = await _licenseRepository.GetByCustomerEmailAsync(customerEmail);
            
            var licenseDtos = new List<LicenseDto>();
            foreach (var license in licenses)
            {
                licenseDtos.Add(new LicenseDto
                {
                    LicenseKey = license.LicenseKey,
                    ProductName = license.ProductName,
                    CreatedDate = license.CreatedDate,
                    ExpirationDate = license.ExpirationDate,
                    IsActive = license.IsActive,
                    IsExpired = license.IsExpired(),
                    ActivationsUsed = license.CurrentActivations,
                    MaxActivations = license.MaxActivations
                });
            }
            
            return licenseDtos;
        }
        
        /// <summary>
        /// Gets licenses that are expiring soon
        /// </summary>
        public async Task<IEnumerable<LicenseDto>> GetExpiringLicensesAsync(int daysThreshold = 30)
        {
            var licenses = await _licenseRepository.GetExpiringLicensesAsync(daysThreshold);
            
            var licenseDtos = new List<LicenseDto>();
            foreach (var license in licenses)
            {
                licenseDtos.Add(new LicenseDto
                {
                    LicenseKey = license.LicenseKey,
                    ProductName = license.ProductName,
                    CustomerEmail = license.CustomerEmail,
                    ExpirationDate = license.ExpirationDate,
                    DaysUntilExpiration = (int)(license.ExpirationDate - DateTime.UtcNow).TotalDays
                });
            }
            
            return licenseDtos;
        }
    }
    
    // Result classes (DTOs)
    
    public class CreateLicenseResult
    {
        public bool Success { get; set; }
        public string? LicenseKey { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public string? ErrorMessage { get; set; }
    }
    
    public class LicenseValidationResult
    {
        public bool IsValid { get; set; }
        public string? Reason { get; set; }
        public License? License { get; set; }
    }
    
    public class LicenseDto
    {
        public string LicenseKey { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public string? CustomerEmail { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsExpired { get; set; }
        public int ActivationsUsed { get; set; }
        public int MaxActivations { get; set; }
        public int? DaysUntilExpiration { get; set; }
    }
}