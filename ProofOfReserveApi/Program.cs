using MerkleCalculator.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;
using ProofOfReserveApi.HealthChecks;
using ProofOfReserveApi.Middleware;
using ProofOfReserveApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMerkleCalculationService();
builder.Services.AddHealthChecks()
    .AddCheck<MaintenanceOngoingHealthCheck>("MaintenanceCheck");

var app = builder.Build();

app.MapHealthChecks("/health");



app.UseMiddleware<OperationStateCheckMiddleware>();

app.MapGet("/", () => "Hello World!");

app.Run();
