using ProofOfReserveApi.Services;
using System.Net;

namespace ProofOfReserveApi.Middleware;

public class OperationStateCheckMiddleware
{
    private readonly RequestDelegate _next;

    public OperationStateCheckMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IMaintenanceStateService maintenanceStateService)
    {
        if (!maintenanceStateService.IsOperating())
        {
            context.Response.StatusCode = (int)HttpStatusCode.ServiceUnavailable;
            return;
        }

        await _next(context);
    }
}
