using Microsoft.AspNetCore.Mvc;
using ProofOfReserveApi.Models;
using ProofOfReserveApi.Services;
using System.Net;

namespace ProofOfReserveApi.Extensions;

public static class AddEndpointsExtension
{
    /// <summary>
    /// Adds endpoints responsinle for service maintenance.
    /// </summary>
    /// <param name="app">The web app.</param>
    public static void MapUpdateEndpoint(this WebApplication app)
    {
        var pushDataEndpoint = app.MapPost("/maintenance/pushdata", async (HttpRequest httpReq, IMaintenanceStateService maintenanceService, IUserBalanceStorage balanceStorage) =>
        {
            maintenanceService.StartMaintenance();

            using StreamReader sr = new StreamReader(httpReq.Body);
            string? userBalanceLine;
            do
            {
                userBalanceLine = await sr.ReadLineAsync();
                if (string.IsNullOrWhiteSpace(userBalanceLine))
                {
                    continue;
                }

                balanceStorage.AddOrUpdateUser(userBalanceLine);
            }
            while (!string.IsNullOrEmpty(userBalanceLine));

            maintenanceService.StopMaintenance();
            return Results.Ok();
        });

        pushDataEndpoint.Accepts<string>("text/plain");
        pushDataEndpoint.WithDisplayName("Push user balances");
        pushDataEndpoint.WithDescription("Adds new users to database, and updates balances for existing users");
        pushDataEndpoint.Produces(StatusCodes.Status200OK);
    }

    /// <summary>
    /// Adds endpoints responsible for proof of reserve API.
    /// </summary>
    /// <param name="app">The web app</param>
    public static void MapUserApiEndpoints(this WebApplication app)
    {
        var endpoints = app.MapGroup("/reserve");

        var rootEndpoint = endpoints.MapGet("/root", async (IMerkleProofService merkleProofService) =>
        {
            var merkleRoot = await merkleProofService.GetMerkleRoot();
            if (merkleRoot == null)
            {
                return Results.InternalServerError();
            }

            return Results.Ok(merkleRoot);
        });

        rootEndpoint.WithDisplayName("Merkle root");
        rootEndpoint.WithDescription("Get merkle root for all users.");
        rootEndpoint.Produces(StatusCodes.Status200OK, typeof(MerkleRootApiModel));

        var proofEndpoint = endpoints.MapGet("/proof/{userId:int}", async ([FromRoute]int userId, IUserBalanceStorage storage, IMerkleProofService proofService) =>
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

        proofEndpoint.WithDisplayName("Merkle Proof");
        proofEndpoint.WithDescription("Gets proof of reserve for the specific user");
        proofEndpoint.Produces(StatusCodes.Status200OK, typeof(ProofOfReserveApiModel));
        proofEndpoint.Produces(StatusCodes.Status404NotFound);
    }
}
