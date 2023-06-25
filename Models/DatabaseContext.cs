namespace ShopAPI.Models
{
    using Microsoft.EntityFrameworkCore;
    public class DatabaseContext : DbContext
    {
        public DbSet<Guitar> Guitars { get; set; } = null!;
        public DbSet<GuitarCategory> GuitarCategories { get; set; } = null!;
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GuitarCategory>()
                .HasMany(g => g.Guitars)
                .WithOne(g => g.Category)
                .HasForeignKey(s => s.CategoryId);
        }
        public DatabaseContext()
        {

        }

    }
}
