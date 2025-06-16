using Security_Software_Distribution_System.SecurityDistribution.Domain.Enums;

namespace Security_Software_Distribution_System.SecurityDistribution.Domain.Entities;

/*
 * ### Security Monitoring
   - Detect and log suspicious activities such as
        - Attempts to exceed activation limits
        - Usage of expired licenses
        - Unusual deployment patterns
   - Provide a way to query security events
 */
public class SecurityEvent(DateTime timestamp, string licenseKey, string details, string ipAddress, SecurityEventType eventType)
{
    public SecurityEventType EventType { get; private set; } = eventType;
    public DateTime Timestamp { get; private set; } = timestamp;
    public string LicenseKey { get; private set; } = licenseKey;
    public string Details { get; private set; } = details;
    public string IpAddress { get; private set; } = ipAddress;
}