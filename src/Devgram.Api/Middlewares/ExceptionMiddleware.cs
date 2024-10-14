using System.Net;
using System.Text.Json;
using Devgram.Data.Interfaces;

namespace Devgram.Api.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task Invoke(HttpContext context, INotifiable _notifiable)
    {
        try
        {
            if (_notifiable.HasNotification)
                await HandleBadRequestAsync(context, _notifiable);
            
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleBadRequestAsync(HttpContext context, INotifiable _notifiable)
    {
        var result = JsonSerializer.Serialize(new { error = _notifiable.GetNotifications });
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        return context.Response.WriteAsync(result);
    }
    
    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {

        var result = JsonSerializer.Serialize(new { error = exception.Message });
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        return context.Response.WriteAsync(result);
    }
}