using gamestore.Entities;
using gamestore.Genre;
using Microsoft.EntityFrameworkCore;

namespace gamestore.Data
{
    public class GamestoreDBContext : DbContext
    {
        public DbSet<Game> Games => Set<Game>();
        public DbSet<GenreType> Genre => Set<GenreType>();

        public GamestoreDBContext(DbContextOptions<GamestoreDBContext> options) : base(options)
        {
        }
    }
}
