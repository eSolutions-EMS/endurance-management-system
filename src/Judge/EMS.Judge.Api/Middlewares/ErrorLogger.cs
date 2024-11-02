using System;
using System.Threading.Tasks;
using EMS.Judge.Api.Services;
using Microsoft.AspNetCore.Http;

namespace EMS.Judge.Api.Middlewares;

public class ErrorLogger : IMiddleware
{
    private readonly ILogger logger;

    public ErrorLogger(ILogger logger)
    {
        this.logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next.Invoke(context);
        }
        catch (Exception exception)
        {
            this.logger.LogError(exception);
        }
    }
}
