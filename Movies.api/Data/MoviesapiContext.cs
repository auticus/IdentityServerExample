using Microsoft.EntityFrameworkCore;
using Movies.api.Models;

namespace Movies.api.Data
{
    public class MoviesapiContext : DbContext
    {
        public MoviesapiContext (DbContextOptions<MoviesapiContext> options)
            : base(options)
        {
        }

        public DbSet<Movie> Movie { get; set; }
    }
}
