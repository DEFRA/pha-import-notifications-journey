using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace PhaImportNotifications.Tests.Clients;

public sealed record PhaImportNotificationsClientOptions
{
    [Required]
    public string BaseUrl { get; init; } = "http://localhost:8080/";

    public string CdpDeveloperApiKey { get; init; } = string.Empty;

    public int TimeoutSeconds { get; init; } = 30;

    public Uri BaseUri => new(BaseUrl);

    public TimeSpan Timeout => TimeSpan.FromSeconds(TimeoutSeconds);

    public static PhaImportNotificationsClientOptions FromConfiguration()
    {
        var configuration = ConfigurationFactory.Build();

        var services = new ServiceCollection();
        services
            .AddOptions<PhaImportNotificationsClientOptions>()
            .Bind(configuration.GetSection("PhaImportNotifications"))
            .ValidateDataAnnotations()
            .Validate(
                options => Uri.TryCreate(options.BaseUrl, UriKind.Absolute, out _) && options.BaseUrl.EndsWith('/'),
                "PhaImportNotifications:BaseUrl must be an absolute URI and end with a trailing slash (for example http://localhost:8080/)."
            );

        var provider = services.BuildServiceProvider();
        var options = provider.GetRequiredService<IOptions<PhaImportNotificationsClientOptions>>();
        return options.Value;
    }
}
