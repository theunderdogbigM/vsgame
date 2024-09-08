using Microsoft.EntityFrameworkCore;

namespace gamestore.Data;

public static class DataExtensions
{
    public static async Task MigrateDbAsync(this WebApplication app)
    {
       using var scope = app.Services.CreateScope();
       var dbContext = scope.ServiceProvider.GetRequiredService<GamestoreDBContext>();
       await dbContext.Database.MigrateAsync();

    }
}
