namespace Security_Software_Distribution_System.SecurityDistribution.Domain.Entities;

/*
 * ### Deployment Tracking
   - Track where and when software is deployed
   - Each deployment should be associated with a specific machine
   - Support activation and deactivation of deployments
   - Prevent unauthorized or excessive deployments
 */
public class Activation(
    string activationKey,
    string licenceKey,
    string machineName,
    string machineId,
    DateTime activationDate,
    DateTime deactivationDate,
    string ipAddress,
    bool isActive)
{
    public string ActivationKey { get; private set; } = activationKey; // (GUID)
    public string LicenceKey { get; private set; } = licenceKey;
    public string MachineName { get; private set; } = machineName;
    public string MachineId { get; private set; } = machineId; // (simulated hardware ID)
    public DateTime ActivationDate { get; private set; } = activationDate;
    public DateTime DeactivationDate { get; private set; } = deactivationDate;
    public string IpAddress { get; private set; } = ipAddress;
    public bool IsActive { get; private set; } = isActive;
}