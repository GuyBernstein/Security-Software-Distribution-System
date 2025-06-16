

using Security_Software_Distribution_System.SecurityDistribution.Domain.Entities;

namespace Security_Software_Distribution_System.SecurityDistribution.Application.Interfaces;

public interface ISecurityEventRepository
{
        
    // Security & Analytics
    void LogSecurityEvent(SecurityEvent securityEventEvent);
    List<SecurityEvent> GetSecurityEvents(DateTime from, DateTime to);
    Dictionary<string, int> GetActivationStatsByProduct();
    List<License> GetExpiringLicenses(int daysThreshold);
}