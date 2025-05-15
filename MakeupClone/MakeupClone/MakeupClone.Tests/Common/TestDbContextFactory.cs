using MakeupClone.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MakeupClone.Tests.Common;

public static class TestDbContextFactory
{
    public static MakeupCloneDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<MakeupCloneDbContext>()
            .UseNpgsql("Host=localhost;Port=5433;Database=MakeupCloneTestDb;Username=userForTests;Password=hnYN7~8/1@_a")
            .Options;

        var context = new MakeupCloneDbContext(options);
        context.Database.EnsureCreated();

        return context;
    }

    public static void ClearDatabase(MakeupCloneDbContext context)
    {
        context.Database.ExecuteSqlRaw("TRUNCATE TABLE \"Brands\", \"Categories\", \"Products\", \"Reviews\" RESTART IDENTITY CASCADE;");
    }
}