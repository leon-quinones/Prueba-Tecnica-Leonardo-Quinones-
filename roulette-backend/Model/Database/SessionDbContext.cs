using Microsoft.EntityFrameworkCore;

namespace Roulette.App.Model.Database
{
    public class SessionDbContext : DbContext
    {
        public DbSet<PlayerSession> Sessions { get; set; }
        public SessionDbContext(DbContextOptions<SessionDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Game>()
                .HasKey(g => ');
        }

    }
}
