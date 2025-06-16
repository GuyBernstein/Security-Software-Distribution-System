using Security_Software_Distribution_System.Model;
using Security_Software_Distribution_System.Program;

namespace Security_Software_Distribution_System.Repository;

public interface ISecurityEventRepository
{
        
    // Security & Analytics
    void LogSecurityEvent(SecurityEvent securityEventEvent);
    List<SecurityEvent> GetSecurityEvents(DateTime from, DateTime to);
    Dictionary<string, int> GetActivationStatsByProduct();
    List<License> GetExpiringLicenses(int daysThreshold);
}