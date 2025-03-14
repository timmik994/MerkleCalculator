using Microsoft.Extensions.Diagnostics.HealthChecks;
using ProofOfReserveApi.Services;

namespace ProofOfReserveApi.HealthChecks;

public class MaintenanceOngoingHealthCheck : IHealthCheck
{
    private readonly IMaintenanceStateService maintenanceStateService;

    public MaintenanceOngoingHealthCheck(IMaintenanceStateService maintenanceStateService)
    {
        this.maintenanceStateService = maintenanceStateService;
    }

    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        var healthStatus = maintenanceStateService.IsOperating() ? HealthStatus.Healthy : HealthStatus.Unhealthy;
        return Task.FromResult(new HealthCheckResult(healthStatus));
    }
}
