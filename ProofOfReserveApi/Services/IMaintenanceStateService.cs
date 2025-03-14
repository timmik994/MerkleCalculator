namespace ProofOfReserveApi.Services;

public interface IMaintenanceStateService
{
    void StartMaintenance();
    void StopMaintenance();
    bool IsOperating();
}
