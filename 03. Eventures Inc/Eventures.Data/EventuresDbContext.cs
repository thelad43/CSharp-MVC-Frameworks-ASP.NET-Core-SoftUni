namespace Eventures.Data
{
    using Eventures.Models;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    public class EventuresDbContext : IdentityDbContext
    {
        public EventuresDbContext(DbContextOptions<EventuresDbContext> options)
            : base(options)
        {
        }

        public DbSet<Event> Events { get; set; }

        public DbSet<Order> Orders { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            base.OnConfiguring(builder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .Entity<User>()
                .HasMany(u => u.Orders)
                .WithOne(o => o.User)
                .HasForeignKey(o => o.UserId);

            builder
                .Entity<Event>()
                .HasMany(e => e.Orders)
                .WithOne(o => o.Event)
                .HasForeignKey(o => o.EventId);

            base.OnModelCreating(builder);
        }
    }
}