using Security_Software_Distribution_System.SecurityDistribution.Domain.Entities;

namespace Security_Software_Distribution_System.SecurityDistribution.Application.Interfaces
{
    /// <summary>
    /// Repository interface for License persistence
    /// </summary>
    public interface ILicenseRepository
    {
        /// <summary>
        /// Adds a new license to the repository
        /// </summary>
        Task<License> AddAsync(License license);
        
        /// <summary>
        /// Gets a license by its key
        /// </summary>
        Task<License?> GetByKeyAsync(string licenseKey);
        
        /// <summary>
        /// Gets all licenses for a customer
        /// </summary>
        Task<IEnumerable<License>> GetByCustomerEmailAsync(string customerEmail);
        
        /// <summary>
        /// Updates an existing license
        /// </summary>
        Task UpdateAsync(License license);
        
        /// <summary>
        /// Gets licenses expiring within the specified days
        /// </summary>
        Task<IEnumerable<License>> GetExpiringLicensesAsync(int daysThreshold);
        
        /// <summary>
        /// Checks if a license key exists
        /// </summary>
        Task<bool> ExistsAsync(string licenseKey);
    }
}