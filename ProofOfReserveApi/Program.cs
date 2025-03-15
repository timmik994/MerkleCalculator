using MerkleCalculator.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
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

builder.Services.AddOpenApi("ServiceMaintenance", options =>
{
    options.ShouldInclude = (ApiDescription description) => description.RelativePath?.StartsWith("maintenance") ?? false;
});

builder.Services.AddOpenApi("ProofOfReserve", options =>
{
    options.ShouldInclude = (ApiDescription description) => description.RelativePath?.StartsWith("reserve") ?? false;
});

var app = builder.Build();
app.MapHealthChecks("/health");

app.MapOpenApi();

app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/openapi/ServiceMaintenance.json", "ServiceMaintenance.json");
    options.SwaggerEndpoint("/openapi/ProofOfReserve.json", "ProofOfReserve.json");
});

app.UseMiddleware<OperationStateCheckMiddleware>();

app.MapUpdateEndpoint();
app.MapUserApiEndpoints();

app.Run();
