using Microsoft.AspNetCore.Mvc.RazorPages;
using gamestore.Data;
using gamestore.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

public class IndexModel : PageModel
{
    private readonly GamestoreDBContext _dbContext;

    // Constructor to inject the database context
    public IndexModel(GamestoreDBContext dbContext)
    {
        _dbContext = dbContext;
    }

    // Property to hold the list of games
    public List<Game> Games { get; set; }

    // This method is called when the page is accessed via GET
    public async Task OnGetAsync()
    {
        // Fetch the list of games from the database
        Games = await _dbContext.Games.ToListAsync();
    }
}
