using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Rimovie.Excepciones;
using System;
using System.Threading.Tasks;

namespace Rimovie.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IWebHostEnvironment _env;

        public ExceptionMiddleware(RequestDelegate next, IWebHostEnvironment env)
        {
            _next = next;
            _env = env;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                context.Response.ContentType = "application/json";

                var statusCode = ex is AppException appEx
                    ? appEx.StatusCode
                    : StatusCodes.Status500InternalServerError;

                context.Response.StatusCode = statusCode;

                var errorResponse = new
                {
                    error = ex.Message,
                    detail = _env.IsDevelopment() ? ex.StackTrace : null
                };

                await context.Response.WriteAsJsonAsync(errorResponse);
            }
        }
    }
}
