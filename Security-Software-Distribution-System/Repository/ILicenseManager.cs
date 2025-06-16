using Security_Software_Distribution_System.Model;

namespace Security_Software_Distribution_System.Repository;

public interface ILicenseManager
{
    License CreateLicense(string productName, string customerEmail, int validityDays, int maxActivations);
    License GetLicense(string licenseKey);
    bool ValidateLicenseKey(string licenseKey);
}