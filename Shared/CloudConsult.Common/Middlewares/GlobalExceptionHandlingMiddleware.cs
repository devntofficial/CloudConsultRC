using CloudConsult.Common.Builders;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CloudConsult.Common.Middlewares;

public class GlobalExceptionHandlingMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            var output = new ApiResponseBuilder<object>();
            output.WithErrorCode(StatusCodes.Status500InternalServerError);
            output.WithErrors(ex.Message);

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";
            var json = JsonSerializer.Serialize(output, new JsonSerializerOptions
            {
                PropertyNamingPolicy = null,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            });


            await context.Response.WriteAsync(json);
        }
    }
}