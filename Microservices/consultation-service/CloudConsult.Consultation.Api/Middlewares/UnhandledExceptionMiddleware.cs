using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using CloudConsult.Common.Builders;
using Microsoft.AspNetCore.Http;

namespace CloudConsult.Consultation.Api.Middlewares
{
    public class UnhandledExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public UnhandledExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                var output = new ApiResponseBuilder<object>();
                output.WithErrorCode(StatusCodes.Status500InternalServerError);
                output.WithErrors(ex.Message);

                var response = httpContext.Response;
                response.StatusCode = StatusCodes.Status500InternalServerError;

                await response.WriteAsJsonAsync(output, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = null,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                });
            }
        }
    }
}