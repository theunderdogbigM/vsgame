using Microsoft.EntityFrameworkCore;

namespace gamestore.Data;

public static class DataExtensions
{
    public static void MigrateDb(this WebApplication app)
    {
       using var scope = app.Services.CreateScope();
       var dbContext = scope.ServiceProvider.GetRequiredService<GamestoreDBContext>();
       dbContext.Database.Migrate();

    }
}
