using Security_Software_Distribution_System.Program;

namespace Security_Software_Distribution_System.Repository;

public interface IActivationManagement
{
    Activation ActivateLicense(string licenseKey, string machineName, string machineId, string ipAddress);
    bool DeactivateLicense(string licenseKey, string machineId);
    List<Activation> GetActivations(string licenseKey);
}