using Security_Software_Distribution_System.SecurityDistribution.Application.Interfaces;
using Security_Software_Distribution_System.SecurityDistribution.Domain.Entities;

namespace Security_Software_Distribution_System.SecurityDistribution.Infrastructure.Repositories
{
    /// <summary>
    /// In-memory implementation of ILicenseRepository
    /// </summary>
    public class InMemoryLicenseRepository : ILicenseRepository
    {
        // Thread-safe dictionary to store licenses
        private readonly Dictionary<string, License> _licenses = new();
        private readonly object _lock = new();
        
        public Task<License> AddAsync(License license)
        {
            if (license == null)
                throw new ArgumentNullException(nameof(license));
            
            lock (_lock)
            {
                if (_licenses.ContainsKey(license.LicenseKey))
                    throw new InvalidOperationException($"License with key {license.LicenseKey} already exists");
                
                _licenses[license.LicenseKey] = license;
            }
            
            return Task.FromResult(license);
        }
        
        public Task<License?> GetByKeyAsync(string licenseKey)
        {
            if (string.IsNullOrWhiteSpace(licenseKey))
                return Task.FromResult<License?>(null);
            
            lock (_lock)
            {
                _licenses.TryGetValue(licenseKey, out var license);
                return Task.FromResult(license);
            }
        }
        
        public Task<IEnumerable<License>> GetByCustomerEmailAsync(string customerEmail)
        {
            if (string.IsNullOrWhiteSpace(customerEmail))
                return Task.FromResult<IEnumerable<License>>(Enumerable.Empty<License>());
            
            lock (_lock)
            {
                var licenses = _licenses.Values
                    .Where(l => l.CustomerEmail.Equals(customerEmail, StringComparison.OrdinalIgnoreCase))
                    .ToList();
                
                return Task.FromResult<IEnumerable<License>>(licenses);
            }
        }
        
        public Task UpdateAsync(License license)
        {
            if (license == null)
                throw new ArgumentNullException(nameof(license));
            
            lock (_lock)
            {
                if (!_licenses.ContainsKey(license.LicenseKey))
                    throw new InvalidOperationException($"License with key {license.LicenseKey} not found");
                
                _licenses[license.LicenseKey] = license;
            }
            
            return Task.CompletedTask;
        }
        
        public Task<IEnumerable<License>> GetExpiringLicensesAsync(int daysThreshold)
        {
            if (daysThreshold < 0)
                throw new ArgumentException("Days threshold must be non-negative", nameof(daysThreshold));
            
            var thresholdDate = DateTime.UtcNow.AddDays(daysThreshold);
            
            lock (_lock)
            {
                var expiringLicenses = _licenses.Values
                    .Where(l => l.IsActive && l.ExpirationDate <= thresholdDate && !l.IsExpired())
                    .ToList();
                
                return Task.FromResult<IEnumerable<License>>(expiringLicenses);
            }
        }
        
        public Task<bool> ExistsAsync(string licenseKey)
        {
            if (string.IsNullOrWhiteSpace(licenseKey))
                return Task.FromResult(false);
            
            lock (_lock)
            {
                return Task.FromResult(_licenses.ContainsKey(licenseKey));
            }
        }
    }
}