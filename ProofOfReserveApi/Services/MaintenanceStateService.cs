namespace ProofOfReserveApi.Services;

public class MaintenanceStateService : IMaintenanceStateService
{
    private volatile bool serviceActive = true;

    private readonly ILogger logger;

    public MaintenanceStateService(ILogger<MaintenanceStateService> logger)
    {
        this.logger = logger;
    }

    public bool IsOperating()
    {
        return serviceActive;
    }

    public void StartMaintenance()
    {
        logger.LogInformation("Maintenance started.");
        serviceActive = false;
    }

    public void StopMaintenance()
    {
        logger.LogInformation("Maintenance finished.");
        serviceActive = true;
    }
}
