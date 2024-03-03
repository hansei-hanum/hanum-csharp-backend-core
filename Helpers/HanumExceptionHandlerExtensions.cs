
using Hanum.Core.Models.Responses;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.Hosting;

namespace Hanum.Core.Helpers;

public static class HanumExceptionHandlerExtensions {
    public static IApplicationBuilder UseHanumExceptionHandler(this WebApplication app) {
        if (app.Environment.IsProduction()) {
            app.UseExceptionHandler(c => c.Run(async context => {
                var exception = context.Features.Get<IExceptionHandlerPathFeature>()?.Error;

                context.Response.StatusCode = 500;
                await context.Response.WriteAsJsonAsync(APIResponse<string>.FromError(exception?.Message ?? "An error occurred"));
            }));
        }

        return app;
    }
}