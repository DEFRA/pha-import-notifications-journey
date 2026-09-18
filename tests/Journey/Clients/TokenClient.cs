using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace PhaImportNotifications.Tests.Clients;

public sealed class TokenClient(HttpClient httpClient, TokenClientOptions options)
{
    public static TokenClient Create()
    {
        var options = TokenClientOptions.FromConfiguration();

        var httpClient = new HttpClient { BaseAddress = options.TokenUri, Timeout = options.Timeout };

        return new TokenClient(httpClient, options);
    }

    public async Task<string> GetAccessTokenAsync(CancellationToken cancellationToken = default)
    {
        var form = new List<KeyValuePair<string, string>>
        {
            new("grant_type", "client_credentials"),
            new("client_id", options.ClientId),
        };

        if (!string.IsNullOrWhiteSpace(options.ClientSecret))
        {
            form.Add(new KeyValuePair<string, string>("client_secret", options.ClientSecret));
        }

        using var content = new FormUrlEncodedContent(form);
        using var response = await httpClient.PostAsync("oauth2/token", content, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);
            throw new InvalidOperationException(
                $"Getting a token from {options.TokenUri}oauth2/token failed with status code "
                    + $"{response.StatusCode} ({(int)response.StatusCode}). Response body: {responseBody}"
            );
        }

        var token =
            await response.Content.ReadFromJsonAsync<TokenResponse>(cancellationToken)
            ?? throw new InvalidOperationException("Token response was empty");

        return token.AccessToken;
    }

    private sealed record TokenResponse
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; init; } = string.Empty;
    }
}
