using Microsoft.EntityFrameworkCore;

namespace Roulette.App.Model.Database
{
    public class RouletteDbContext : DbContext
    {
        public DbSet<Game> Games { get; set; }
        public DbSet<Wager> Wagers { get; set; }
        public DbSet<PlayerAccount> Players { get; set; }
        public DbSet<Result> GameResults { get; set; }

        public RouletteDbContext(DbContextOptions<RouletteDbContext> options)
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
                .HasKey(g => g.Id);

            modelBuilder.Entity<PlayerAccount>()
                .HasKey(p => p.Username);

            modelBuilder.Entity<Wager>()
                .HasKey(w => w.Id);

            modelBuilder.Entity<Result>()
                .HasKey(r => r.Id);

            modelBuilder.Entity<Game>()                
                .HasMany(g => g.Wagers)
                .WithOne(w => w.Game)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Game>()
            .Property(g => g.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

            modelBuilder.Entity<Wager>()
                .HasOne<PlayerAccount>( w => w.PlayerAccount)
                .WithMany(p => p.Wagers)
                .HasForeignKey(w => w.Username)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Wager>()
                .Property(w => w.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            modelBuilder.Entity<Wager>()
                .Property(w => w.IncludedInBalance)
                .HasDefaultValueSql("False");
        }

    }
}
