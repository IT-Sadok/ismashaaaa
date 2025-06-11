using MakeupClone.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MakeupClone.Tests.Common;

public static class TestDbContextFactory
{
    public static MakeupCloneDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<MakeupCloneDbContext>()
            .UseNpgsql(TestConfiguration.ConnectionString)
            .Options;

        var context = new MakeupCloneDbContext(options);

        return context;
    }

    public static void ClearDatabase(MakeupCloneDbContext context)
    {
        context.Database.ExecuteSqlRaw("""
        TRUNCATE TABLE 
            "AspNetUserRoles",
            "AspNetUserClaims",
            "AspNetUserLogins",
            "AspNetUserTokens",
            "AspNetUsers",
            "AspNetRoleClaims",
            "AspNetRoles",
            "Reviews",
            "Products",
            "Categories",
            "Brands",
            "Orders",
            "Items"
        RESTART IDENTITY CASCADE;
    """);
    }
}