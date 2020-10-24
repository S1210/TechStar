using Microsoft.EntityFrameworkCore;
using TechStar.Models;

namespace TechStar.Context
{
    public class PostgersContext : DbContext
    {
        public DbSet<Word> Words { get; set; }
        public DbSet<Log> Logs { get; set; }

        public PostgersContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=techstar;Username=postgres;Password=admin");
        }
    }
}
