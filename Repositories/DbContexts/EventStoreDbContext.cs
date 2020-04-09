using Microsoft.EntityFrameworkCore;
using Repositories.EventStoreEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repositories.DbContexts
{
    /// <summary>
    /// Event sourcing db context.
    /// </summary>
    public class EventStoreDbContext : DbContext
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="options"></param>
        public EventStoreDbContext(DbContextOptions<EventStoreDbContext> options)
            : base(options)
        {
            ChangeTracker.LazyLoadingEnabled = false;
        }

        /// <summary>
        /// Events.
        /// </summary>
        public DbSet<Event> Events { get; set; }

        /// <summary>
        /// Aggregates.
        /// </summary>
        public DbSet<Aggregate> Aggregates { get; set; }

        /// <summary>
        /// Fluent API configuration.
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Event>()
                .HasKey(e => e.Id);
            modelBuilder.Entity<Event>()
                .Property(e => e.Datas)
                .IsRequired();
            modelBuilder.Entity<Event>()
                .HasOne(e => e.Aggregate)
                .WithMany(a => a.Events)
                .HasForeignKey(e => e.AggregateId)
                .IsRequired();

            modelBuilder.Entity<Aggregate>()
                .HasKey(es => es.Id);
        }
    }
}
