using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using FileStorage.Common.Exceptions;

namespace FileStorage.Common.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        context.Response.ContentType = "application/json";
        try
        {
            await _next(context);
        }
        catch (ApiException apiException)
        {
            context.Response.StatusCode = (int)apiException.GetStatusCode();
            await context.Response.WriteAsync(JsonSerializer.Serialize(new { message = apiException.Message }));
        }
        catch (Exception e)
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await context.Response.WriteAsync(JsonSerializer.Serialize(new { message = "Internal Server Error" }));
        }
    }
}

public static class ExceptionMiddlewareExtensions
{
    public static IApplicationBuilder UseCustomExceptionMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionMiddleware>();
    }
}