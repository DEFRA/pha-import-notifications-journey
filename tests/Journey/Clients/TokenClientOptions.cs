using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace PhaImportNotifications.Tests.Clients;

public sealed record TokenClientOptions
{
    [Required]
    public string TokenUrl { get; init; } = "http://localhost:8080/";

    [Required]
    public string ClientId { get; init; } = string.Empty;

    public string ClientSecret { get; init; } = string.Empty;

    public int TimeoutSeconds { get; init; } = 30;

    public Uri TokenUri => new(TokenUrl);

    public TimeSpan Timeout => TimeSpan.FromSeconds(TimeoutSeconds);

    public static TokenClientOptions FromConfiguration()
    {
        var configuration = ConfigurationFactory.Build();

        var services = new ServiceCollection();
        services
            .AddOptions<TokenClientOptions>()
            .Bind(configuration.GetSection("Token"))
            .ValidateDataAnnotations()
            .Validate(
                options => Uri.TryCreate(options.TokenUrl, UriKind.Absolute, out _) && options.TokenUrl.EndsWith('/'),
                "Token:TokenUrl must be an absolute URI and end with a trailing slash (for example http://localhost:8080/)."
            );

        var provider = services.BuildServiceProvider();
        var options = provider.GetRequiredService<IOptions<TokenClientOptions>>();
        return options.Value;
    }
}
