using Security_Software_Distribution_System.Program;

namespace Security_Software_Distribution_System.Model;


/*
 * ### License Management
   - The system should support creating and managing software licenses
   - Each license should have an activation limit and expiration period
   - Licenses should be associated with customers
   - The system must validate license authenticity and status
 */
/// Represents a software license with activation and expiration management
public class License(
    string licenseKey,
    string productName,
    DateTime activationLimit,
    DateTime expirationPeriod,
    string customerEmail,
    int maxActivations,
    bool status)
{
    // (format for the key: XXXX-XXXX-XXXX-XXXX where X is alphanumeric)
    public string LicenseKey { get; private set; } = licenseKey;
    public string ProductName { get; private set; } = productName;
    public DateTime ActivationLimit { get; private set; } = activationLimit;
    public DateTime ExpirationPeriod { get; private set; } = expirationPeriod;
    public string CustomerEmail { get; private set; } = customerEmail;
    public int MaxActivations { get; private set; } = maxActivations;
    public bool Status { get; private set; } = status;
}