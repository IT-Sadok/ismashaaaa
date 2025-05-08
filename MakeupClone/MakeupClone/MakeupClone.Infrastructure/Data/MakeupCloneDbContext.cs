using MakeupClone.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace MakeupClone.Infrastructure.Data
{
    public class MakeupCloneDbContext : DbContext
    {
        public MakeupCloneDbContext(DbContextOptions<MakeupCloneDbContext> options) : base(options)
        { }

        public DbSet<ProductEntity> Products { get; set; }

        public DbSet<CategoryEntity> Categories { get; set; }

        public DbSet<BrandEntity> Brands { get; set; }

        public DbSet<ReviewEntity> Reviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ProductEntity>()
                .Property(product => product.Price)
                .HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<CategoryEntity>()
                .HasIndex(category => category.Name)
                .IsUnique();

            modelBuilder.Entity<BrandEntity>()
                .HasIndex(brand => brand.Name)
                .IsUnique();

            modelBuilder.Entity<ReviewEntity>()
                .Property(review => review.Rating)
                .HasConversion<int>()
                .IsRequired();

            modelBuilder.Entity<ReviewEntity>()
              .HasIndex(review => new { review.ProductId, review.UserId });
        }
    }
}
