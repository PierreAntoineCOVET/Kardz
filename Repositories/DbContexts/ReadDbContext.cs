using Microsoft.EntityFrameworkCore;
using Repositories.ReadEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repositories.DbContexts
{
    public class ReadDbContext : DbContext
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="options"></param>
        public ReadDbContext(DbContextOptions<ReadDbContext> options)
            : base(options)
        {
            ChangeTracker.LazyLoadingEnabled = false;
        }

        public DbSet<CoincheGame> CoincheGames { get; set; }

        public DbSet<CoincheTeam> CoincheTeams { get; set; }

        public DbSet<CoinchePlayer> CoinchePlayers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CoinchePlayer>()
                .HasKey(p => p.Id);
            modelBuilder.Entity<CoinchePlayer>()
                .Property(p => p.Cards)
                .IsUnicode(false)
                .HasMaxLength(23);
            modelBuilder.Entity<CoinchePlayer>()
                .HasOne(p => p.Team)
                .WithMany(t => t.Players)
                .HasForeignKey(p => new { p.GameId, p.TeamNumber });

            modelBuilder.Entity<CoincheTeam>()
                .HasKey(t => new { t.GameId, t.Number });
            modelBuilder.Entity<CoincheTeam>()
                .HasOne(t => t.Game)
                .WithMany(g => g.Teams)
                .HasForeignKey(t => t.GameId);

            modelBuilder.Entity<CoincheGame>()
                .HasKey(g => g.Id);
            modelBuilder.Entity<CoincheGame>()
                .Property(p => p.CurrentCards)
                .IsUnicode(false)
                .HasMaxLength(8);
        }
    }
}
