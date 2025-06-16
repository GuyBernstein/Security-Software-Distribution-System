
using Security_Software_Distribution_System.SecurityDistribution.Domain.Entities;

namespace Security_Software_Distribution_System.SecurityDistribution.Application.Interfaces;

public interface ILicenseRepository
{
    License CreateLicense(string productName, string customerEmail, int validityDays, int maxActivations);
    License GetLicense(string licenseKey);
    bool ValidateLicenseKey(string licenseKey);
}