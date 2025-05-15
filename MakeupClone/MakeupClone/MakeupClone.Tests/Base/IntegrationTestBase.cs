using AutoMapper;
using MakeupClone.Infrastructure.Data;
using MakeupClone.Infrastructure.Data.MappingProfiles;
using MakeupClone.Tests.Common;

namespace MakeupClone.Tests.Base;

public abstract class IntegrationTestBase : IDisposable
{
    private readonly MakeupCloneDbContext dbContext;
    private readonly IMapper mapper;

    protected IntegrationTestBase()
    {
        dbContext = TestDbContextFactory.CreateDbContext();
        mapper = InitializeMapper();

        InitializeData(dbContext);
    }

    protected virtual IMapper InitializeMapper()
    {
        var config = new MapperConfiguration(configuration =>
        {
            configuration.AddProfile<MappingProfile>();
        });
        return config.CreateMapper();
    }

    protected abstract void InitializeData(MakeupCloneDbContext context);

    public void Dispose()
    {
        dbContext.Database.EnsureDeleted();
        dbContext.Dispose();
    }
}