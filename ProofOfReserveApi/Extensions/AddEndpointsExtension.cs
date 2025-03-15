using ProofOfReserveApi.Services;

namespace ProofOfReserveApi.Extensions;

public static class AddEndpointsExtension
{
    public static void MapUpdateEndpoint(this WebApplication app)
    {
        app.MapPost("/users/update", async (HttpRequest httpReq, IMaintenanceStateService maintenanceService, IUserBalanceStorage balanceStorage) =>
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

    public static void MapUserApiEndpoints(this WebApplication app) 
    {
        var endpoints = app.MapGroup("/reserve");

        endpoints.MapGet("/root", async (IMerkleProofService merkleProofService) =>
        {
            var merkleRoot = await merkleProofService.GetMerkleRoot();
            if(merkleRoot == null)
            {
                return Results.InternalServerError();
            }

            return Results.Ok(merkleRoot);
        });

        endpoints.MapGet("/proof/{userId:int}", async (int userId, IUserBalanceStorage storage, IMerkleProofService proofService) =>
        {
            if (!storage.UserExists(userId))
            {
                return Results.NotFound();
            }

            var proofData = await proofService.GetMerkleProof(userId);
            if (proofData == null)
            {
                return Results.InternalServerError();
            }

            return Results.Ok(proofData);
        });
    }
}
