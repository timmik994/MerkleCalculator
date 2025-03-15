using MerkleCalculator.Extensions;
using ProofOfReserveApi.Extensions;
using ProofOfReserveApi.HealthChecks;
using ProofOfReserveApi.Middleware;
using ProofOfReserveApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMerkleCalculationService();
builder.Services.AddSingleton<IMaintenanceStateService, MaintenanceStateService>();
builder.Services.AddSingleton<IUserBalanceStorage, UserBalanceStorage>();
builder.Services.AddTransient<IMerkleProofService, MerkleProofService>();
builder.Services.AddHealthChecks()
    .AddCheck<MaintenanceOngoingHealthCheck>("MaintenanceCheck");

var app = builder.Build();
app.MapHealthChecks("/health");

app.UseMiddleware<OperationStateCheckMiddleware>();

app.MapUpdateEndpoint();
app.MapUserApiEndpoints();

app.Run();
