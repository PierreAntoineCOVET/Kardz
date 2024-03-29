﻿using Domain.Entities.ReadEntities;
using Microsoft.EntityFrameworkCore;

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

        /// <summary>
        /// List of currently playing games.
        /// </summary>
        public DbSet<CoincheGame> CoincheGames { get; set; }

        /// <summary>
        /// Game's teams.
        /// </summary>
        public DbSet<CoincheTeam> CoincheTeams { get; set; }

        /// <summary>
        /// Teams's players.
        /// </summary>
        public DbSet<CoinchePlayer> CoinchePlayers { get; set; }

        /// <summary>
        /// Teams's players.
        /// </summary>
        public DbSet<CoincheTake> CoincheTakes { get; set; }

        /// <summary>
        /// Fluent API configuration.
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CoinchePlayer>()
                .HasKey(p => p.Id);
            modelBuilder.Entity<CoinchePlayer>()
                .Property(p => p.Cards)
                .IsUnicode(false)
                .HasMaxLength(23);
            modelBuilder.Entity<CoinchePlayer>()
                .Property(p => p.PlayableCards)
                .IsUnicode(false)
                .HasMaxLength(23);
            modelBuilder.Entity<CoinchePlayer>()
                .HasOne(p => p.Team)
                .WithMany(t => t.Players)
                .HasForeignKey(p => p.TeamId);

            modelBuilder.Entity<CoincheTeam>()
                .HasKey(t => t.Id);
            modelBuilder.Entity<CoincheTeam>()
                .HasOne(t => t.Game)
                .WithMany(g => g.Teams)
                .HasForeignKey(t => t.GameId);

            modelBuilder.Entity<CoincheGame>()
                .HasKey(g => g.Id);
            modelBuilder.Entity<CoincheGame>()
                .Property(g => g.CurrentTurnCards)
                .IsUnicode(false)
                .HasMaxLength(8);
            modelBuilder.Entity<CoincheGame>()
                .Property(g => g.LastTurnCards)
                .IsUnicode(false)
                .HasMaxLength(8);

            modelBuilder.Entity<CoincheTake>()
                .HasKey (t => t.Id);
            modelBuilder.Entity<CoincheTake>()
                .HasOne(t => t.Game)
                .WithMany(g => g.Takes)
                .HasForeignKey (t => t.GameId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<CoincheTake>()
                .HasOne(t => t.CurrentPlayer)
                .WithMany(p => p.Takes)
                .HasForeignKey(t => t.CurrentPlayerId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<CoincheTake>()
                .Property(p => p.CurrentFold)
                .IsUnicode(false)
                .HasMaxLength(23);
            modelBuilder.Entity<CoincheTake>()
                .Property(p => p.CurrentPlayerPlayableCards)
                .IsUnicode(false)
                .HasMaxLength(23);
            modelBuilder.Entity<CoincheTake>()
                .Property(p => p.PreviousFold)
                .IsUnicode(false)
                .HasMaxLength(23);
        }
    }
}
