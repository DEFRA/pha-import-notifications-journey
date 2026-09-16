using System.Globalization;
using System.Net.Http.Headers;

namespace PhaImportNotifications.Tests.Clients;

public sealed class PhaImportNotificationsClient(HttpClient httpClient, TokenClient tokenClient)
{
    private const string UtcFormat = "yyyy-MM-ddTHH:mm:ss'Z'";

    public static PhaImportNotificationsClient Create()
    {
        var options = PhaImportNotificationsClientOptions.FromConfiguration();

        var httpClient = new HttpClient { BaseAddress = options.BaseUri, Timeout = options.Timeout };
        if (!string.IsNullOrWhiteSpace(options.CdpDeveloperApiKey))
        {
            httpClient.DefaultRequestHeaders.Add("x-api-key", options.CdpDeveloperApiKey);
        }

        return new PhaImportNotificationsClient(httpClient, TokenClient.Create());
    }

    public Task<HttpResponseMessage> GetUpdatedImportNotificationsAsync(
        string bcp,
        DateTimeOffset from,
        DateTimeOffset to,
        CancellationToken cancellationToken = default
    )
    {
        var query =
            $"?bcp={Uri.EscapeDataString(bcp)}"
            + $"&from={Uri.EscapeDataString(Format(from))}"
            + $"&to={Uri.EscapeDataString(Format(to))}";

        return SendAsync($"import-notifications{query}", cancellationToken);
    }

    public Task<HttpResponseMessage> GetImportNotificationAsync(
        string chedReferenceNumber,
        CancellationToken cancellationToken = default
    ) => SendAsync($"import-notifications/{Uri.EscapeDataString(chedReferenceNumber)}", cancellationToken);

    private async Task<HttpResponseMessage> SendAsync(string requestUri, CancellationToken cancellationToken)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
        request.Headers.Authorization = new AuthenticationHeaderValue(
            "Bearer",
            await tokenClient.GetAccessTokenAsync(cancellationToken)
        );

        return await httpClient.SendAsync(request, cancellationToken);
    }

    private static string Format(DateTimeOffset value) =>
        value.UtcDateTime.ToString(UtcFormat, CultureInfo.InvariantCulture);
}
