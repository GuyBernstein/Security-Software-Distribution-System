using System.Text.RegularExpressions;

namespace Security_Software_Distribution_System.SecurityDistribution.Domain.Entities
{
    /// <summary>
    /// Represents a software license with validation and business rules
    /// </summary>
    public class License
    {
        // Private fields to ensure encapsulation
        private readonly List<string> _activeMachineIds = [];
        
        // Properties with private setters for immutability
        public string LicenseKey { get; private set; }
        public string ProductName { get; private set; }
        public string CustomerEmail { get; private set; }
        public DateTime CreatedDate { get; private set; }
        public DateTime ExpirationDate { get; private set; }
        public int MaxActivations { get; private set; }
        public bool IsActive { get; private set; }
        
        // Read-only access to active machines
        public IReadOnlyList<string> ActiveMachineIds => _activeMachineIds.AsReadOnly();
        public int CurrentActivations => _activeMachineIds.Count;
        
        // Constructor - notice we generate the key internally
        public License(string productName, string customerEmail, int validityDays, int maxActivations)
        {
            // Validation in constructor
            if (string.IsNullOrWhiteSpace(productName))
                throw new ArgumentException("Product name cannot be empty", nameof(productName));
            
            if (string.IsNullOrWhiteSpace(customerEmail))
                throw new ArgumentException("Customer email cannot be empty", nameof(customerEmail));
            
            if (!IsValidEmail(customerEmail))
                throw new ArgumentException("Invalid email format", nameof(customerEmail));
            
            if (validityDays <= 0)
                throw new ArgumentException("Validity days must be positive", nameof(validityDays));
            
            if (maxActivations <= 0)
                throw new ArgumentException("Max activations must be positive", nameof(maxActivations));
            
            // Initialize properties
            LicenseKey = GenerateLicenseKey();
            ProductName = productName;
            CustomerEmail = customerEmail;
            CreatedDate = DateTime.UtcNow;
            ExpirationDate = CreatedDate.AddDays(validityDays);
            MaxActivations = maxActivations;
            IsActive = true;
        }
        
        // Business methods - these contain domain logic
        
        /// <summary>
        /// Checks if the license can be activated on a new machine
        /// </summary>
        public bool CanActivate(string machineId)
        {
            if (!IsActive) return false;
            if (IsExpired()) return false;
            if (_activeMachineIds.Contains(machineId)) return true; // Already activated
            if (CurrentActivations >= MaxActivations) return false;
            
            return true;
        }
        
        /// <summary>
        /// Activates the license on a machine
        /// </summary>
        public void Activate(string machineId)
        {
            if (string.IsNullOrWhiteSpace(machineId))
                throw new ArgumentException("Machine ID cannot be empty", nameof(machineId));
            
            if (!CanActivate(machineId))
                throw new InvalidOperationException($"Cannot activate license on machine {machineId}");
            
            if (!_activeMachineIds.Contains(machineId))
                _activeMachineIds.Add(machineId);
        }
        
        /// <summary>
        /// Deactivates the license on a machine
        /// </summary>
        public void Deactivate(string machineId)
        {
            if (!_activeMachineIds.Contains(machineId))
                throw new InvalidOperationException($"License is not activated on machine {machineId}");
            
            _activeMachineIds.Remove(machineId);
        }
        
        /// <summary>
        /// Checks if the license is expired
        /// </summary>
        public bool IsExpired()
        {
            return DateTime.UtcNow > ExpirationDate;
        }
        
        /// <summary>
        /// Deactivates the license completely
        /// </summary>
        public void Revoke()
        {
            IsActive = false;
            _activeMachineIds.Clear();
        }
        
        // Private helper methods
        
        private static string GenerateLicenseKey()
        {
            var guid = Guid.NewGuid().ToString("N").ToUpper();
            return $"{guid.Substring(0, 4)}-{guid.Substring(4, 4)}-{guid.Substring(8, 4)}-{guid.Substring(12, 4)}";
        }
        
        private static bool IsValidEmail(string email)
        {
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }
    }
}