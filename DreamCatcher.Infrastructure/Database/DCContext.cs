using DreamCatcher.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace DreamCatcher.Infrastructure.Database
{
    public class DCContext : DbContext
    {
        public DbSet<Dream> Dreams { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=MSI;Initial Catalog=DreamCatcher;Integrated Security=True;TrustServerCertificate=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
