using System.Text.RegularExpressions;
using gamestore.Data;
using gamestore.Mapping;
using Microsoft.EntityFrameworkCore;

namespace gamestore.Entities;

public static class GenreEndpoints
{
   public static RouteGroupBuilder MapGenreEndpoints(this WebApplication app)
   {
    var group  = app.MapGroup("genres");

    group.MapGet("/", async (GamestoreDBContext dbContext) => 
    await dbContext.Genre.Select(genre => genre.ToDto()).AsNoTracking().ToListAsync());

    return group;
   }
}
