using MerkleCalculator.Extensions;
using ProofOfReserveApi.HealthChecks;
using ProofOfReserveApi.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMerkleCalculationService();
builder.Services.AddHealthChecks()
    .AddCheck<MaintenanceOngoingHealthCheck>("MaintenanceCheck");

var app = builder.Build();

app.MapHealthChecks("/health");

app.UseMiddleware<OperationStateCheckMiddleware>();

app.MapGet("/", () => "Hello World!");

app.Run();
