using ProofOfReserveApi.Services;

namespace ProofOfReserveApi.Extensions;

public static class AddEndpointsExtension
{
    public static void AddUpdateEndpoint(this WebApplication app)
    {
        app.MapPost("/balance/update", async (HttpRequest httpReq, IMaintenanceStateService maintenanceService, IUserBalanceStorage balanceStorage) =>
        {
            maintenanceService.StartMaintenance();

            using StreamReader sr = new StreamReader(httpReq.Body);
            while (!sr.EndOfStream)
            {
                string? balanceStr = await sr.ReadLineAsync();
                if (string.IsNullOrWhiteSpace(balanceStr))
                {
                    continue;
                }

                balanceStorage.AddOrUpdateUser(balanceStr);
            }

            maintenanceService.StopMaintenance();
            return Results.Ok();
        });
    }

    public static void AddUserApiEndpoints(this WebApplication app) 
    { 
    }
}
