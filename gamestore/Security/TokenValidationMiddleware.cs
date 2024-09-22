using gamestore.Data;
using gamestore.Entities;
using Microsoft.EntityFrameworkCore;

namespace gamestore.Security;

public class TokenValidationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly GamestoreDBContext _dbContext;

    public TokenValidationMiddleware(RequestDelegate next, GamestoreDBContext dbContext)
    {
        _next = next;
        _dbContext = dbContext;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

        if (!string.IsNullOrEmpty(token))
        {
            var isBlacklisted = await _dbContext.BlacklistedTokens.AnyAsync(bt => bt.Token == token);
            if (isBlacklisted)
            {
                context.Response.StatusCode = 401; // Unauthorized
                await context.Response.WriteAsync("Token has been invalidated.");
                return;
            }
        }

        await _next(context);
    }
}

