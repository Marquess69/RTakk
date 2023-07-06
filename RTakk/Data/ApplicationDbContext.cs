using Microsoft.EntityFrameworkCore;
using RTakk.Models;

namespace RTakk.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(e => { e.HasIndex(u => u.Email).IsUnique(); });
        }
    }
}
