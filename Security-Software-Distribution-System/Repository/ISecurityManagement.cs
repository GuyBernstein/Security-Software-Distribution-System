using Security_Software_Distribution_System.Model;
using Security_Software_Distribution_System.Program;

namespace Security_Software_Distribution_System.Repository;

public interface ISecurityManagement
{
        
    // Security & Analytics
    void LogSecurityEvent(Security securityEvent);
    List<Security> GetSecurityEvents(DateTime from, DateTime to);
    Dictionary<string, int> GetActivationStatsByProduct();
    List<License> GetExpiringLicenses(int daysThreshold);
}