namespace ProofOfReserveApi.Services;

/// <summary>
/// Service to temporarily stop API for maintenance.
/// </summary>
public interface IMaintenanceStateService
{
    /// <summary>
    /// Starts maintenance of the service 
    /// (all new request will have 503 service unavailable response.)
    /// </summary>
    void StartMaintenance();

    /// <summary>
    /// Stops service maintenance and allowe service execution as usual.
    /// </summary>
    void StopMaintenance();

    /// <summary>
    /// Checks is service operating right now.
    /// </summary>
    /// <returns>true, if service healthy, false otherwise.</returns>
    bool IsOperating();
}
