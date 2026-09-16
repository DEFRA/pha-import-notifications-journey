using Microsoft.Extensions.Configuration;

namespace PhaImportNotifications.Tests.Clients;

internal static class ConfigurationFactory
{
    public static IConfigurationRoot Build()
    {
        return new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .AddEnvironmentVariables()
            .Build();
    }
}
