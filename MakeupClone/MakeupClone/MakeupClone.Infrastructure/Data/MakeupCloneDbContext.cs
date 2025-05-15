using MakeupClone.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace MakeupClone.Infrastructure.Data;

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

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MakeupCloneDbContext).Assembly);
    }
}