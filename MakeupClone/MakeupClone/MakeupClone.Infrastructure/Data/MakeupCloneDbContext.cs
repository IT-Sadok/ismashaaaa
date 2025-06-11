using MakeupClone.Domain.Entities;
using MakeupClone.Infrastructure.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MakeupClone.Infrastructure.Data;

public class MakeupCloneDbContext : IdentityDbContext<User, IdentityRole, string>
{
    public MakeupCloneDbContext(DbContextOptions<MakeupCloneDbContext> options) : base(options)
    { }

    public DbSet<ProductEntity> Products { get; set; }

    public DbSet<CategoryEntity> Categories { get; set; }

    public DbSet<BrandEntity> Brands { get; set; }

    public DbSet<ReviewEntity> Reviews { get; set; }

    public DbSet<OrderEntity> Orders { get; set; }

    public DbSet<OrderItemEntity> Items { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MakeupCloneDbContext).Assembly);
    }
}