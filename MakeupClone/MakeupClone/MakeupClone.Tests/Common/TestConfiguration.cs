using Microsoft.Extensions.Configuration;

namespace MakeupClone.Tests.Common;

public static class TestConfiguration
{
    private static IConfiguration? _configuration;

    public static IConfiguration Configuration => _configuration ??= new ConfigurationBuilder()
        .SetBasePath(AppContext.BaseDirectory)
        .AddEnvironmentVariables()
        .AddJsonFile("appsettings.Test.json", optional: true)
        .Build();

    public static string ConnectionString => Configuration.GetConnectionString("MakeupCloneTestDb") !;
}