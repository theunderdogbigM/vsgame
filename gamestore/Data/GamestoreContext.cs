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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GenreType>().HasData(


                new {Id = 1, Name = "Fighting"},
                new {Id = 2, Name = "Adventure"},
                new {Id = 3, Name = "Sports"},
                new {Id = 4, Name = "RPG"},
                new {Id = 5, Name = "Quests"}
            );
        }
    }
}
