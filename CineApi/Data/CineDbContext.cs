using Microsoft.EntityFrameworkCore;
using DemoBlazorMovil.Shared.Models;

namespace CineApi.Data
{
    public class CineDbContext : DbContext
    {
        public CineDbContext(DbContextOptions<CineDbContext> options) : base(options) { }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Showtime> Showtimes { get; set; }
        public DbSet<User> Users { get; set; }
    }
}