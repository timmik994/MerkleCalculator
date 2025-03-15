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
        //We do not apply this middleware for health endpoint.
        if (context.Request.Path.StartsWithSegments("/health"))
        {
            await _next(context);
        }

        if (!maintenanceStateService.IsOperating())
        {
            context.Response.StatusCode = (int)HttpStatusCode.ServiceUnavailable;
            return;
        }

        await _next(context);
    }
}
