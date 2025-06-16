using Security_Software_Distribution_System.SecurityDistribution.Domain.Entities;

namespace Security_Software_Distribution_System.SecurityDistribution.Application.Interfaces;

public interface IActivationRepository
{
    Activation ActivateLicense(string licenseKey, string machineName, string machineId, string ipAddress);
    bool DeactivateLicense(string licenseKey, string machineId);
    List<Activation> GetActivations(string licenseKey);
}