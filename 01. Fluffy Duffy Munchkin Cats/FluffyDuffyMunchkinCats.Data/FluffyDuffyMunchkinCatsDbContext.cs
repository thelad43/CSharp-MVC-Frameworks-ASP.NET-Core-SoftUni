namespace FluffyDuffyMunchkinCats.Data
{
    using Infrastructure;
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class FluffyDuffyMunchkinCatsDbContext : DbContext
    {
        public FluffyDuffyMunchkinCatsDbContext()
        {
        }

        public FluffyDuffyMunchkinCatsDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Cat> Cats { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            if (!builder.IsConfigured)
            {
                builder.UseSqlServer(DbConfiguration.ConnectionString);
            }

            base.OnConfiguring(builder);
        }
    }
}