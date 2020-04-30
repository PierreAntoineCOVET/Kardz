using Domain.Entities.EventStoreEntities;
using Microsoft.EntityFrameworkCore;

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
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(1500);
            modelBuilder.Entity<Event>()
                .Property(e => e.Type)
                .IsUnicode(false)
                .IsRequired()
                .HasMaxLength(200);
            modelBuilder.Entity<Event>()
                .HasOne(e => e.Aggregate)
                .WithMany(a => a.Events)
                .HasForeignKey(e => e.AggregateId)
                .IsRequired();

            modelBuilder.Entity<Aggregate>()
                .HasKey(es => es.Id);
            modelBuilder.Entity<Aggregate>()
                .Property(a => a.Type)
                .IsUnicode(false)
                .IsRequired()
                .HasMaxLength(200);
        }
    }
}
