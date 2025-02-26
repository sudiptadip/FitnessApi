using FitnessApi.Data;
using FitnessApi.Model;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text.Json;

namespace FitnessApi.Middleware
{
    public class OriginValidationMiddleware
    {
        private readonly RequestDelegate _next;

        public OriginValidationMiddleware(RequestDelegate next)
        {
            _next = next;            
        }

        public async Task InvokeAsync(HttpContext context, ApplicationDbContext db)
        {
            var origin2 = context.Request.Headers;
            var origin = context.Request.Headers["Origin"].ToString();

            if(!string.IsNullOrWhiteSpace(origin))
            {
                var isAllowed = await db.AllowedOrigins.AnyAsync(o => o.OriginUrl == origin);
                if(isAllowed)
                {
                    await _next(context);
                }
            }

            
            APIResponse apiResponse = new()
            {
                StatusCode = HttpStatusCode.Forbidden,
                IsSuccess = false,
                ErrorMessages = new List<string> { "Origin not allowed." }
            };

            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            context.Response.ContentType = "application/json";

            var jsonResponse = JsonSerializer.Serialize(apiResponse);
            await context.Response.WriteAsync(jsonResponse);
            return;
        }
    }
}
