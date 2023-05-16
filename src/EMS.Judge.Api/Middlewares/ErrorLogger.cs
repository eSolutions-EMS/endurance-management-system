using Endurance.Judge.Gateways.API.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace Endurance.Judge.Gateways.API.Middlewares
{
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
}
