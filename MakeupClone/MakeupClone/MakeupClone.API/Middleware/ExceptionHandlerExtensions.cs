using Microsoft.AspNetCore.Diagnostics;

namespace MakeupClone.API.Middleware;

public static class ExceptionHandlerExtensions
{
    public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder applicationBuilder)
    {
        applicationBuilder.UseExceptionHandler(errorApplication =>
        {
            errorApplication.Run(async context =>
            {
                var exceptionHandler = context.RequestServices.GetRequiredService<IExceptionHandler>();
                var exceptionFeature = context.Features.Get<IExceptionHandlerFeature>();
                if (exceptionFeature != null)
                {
                    await exceptionHandler.TryHandleAsync(context, exceptionFeature.Error, CancellationToken.None);
                }
            });
        });

        return applicationBuilder;
    }
}