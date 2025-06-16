namespace Security_Software_Distribution_System.Program;

/*
 * ### Deployment Tracking
   - Track where and when software is deployed
   - Each deployment should be associated with a specific machine
   - Support activation and deactivation of deployments
   - Prevent unauthorized or excessive deployments
 */
public class Activation(
    string activationKey,
    string lisenceKey,
    string machineName,
    string machineId,
    DateTime activationDate,
    DateTime deactivationDate,
    string ipAddress,
    bool isActive)
{
    public string ActivationKey { get; private set; } = activationKey; // (GUID)
    public string LisenceKey { get; private set; } = lisenceKey;
    public string MachineName { get; private set; } = machineName;
    public string MachineId { get; private set; } = machineId; // (simulated hardware ID)
    public DateTime ActivationDate { get; private set; } = activationDate;
    public DateTime DeactivationDate { get; private set; } = deactivationDate;
    public string IpAddress { get; private set; } = ipAddress;
    public bool IsActive { get; private set; } = isActive;
}