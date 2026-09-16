using AwesomeAssertions;
using PhaImportNotifications.Tests.Clients;

namespace PhaImportNotifications.Tests;

public abstract class JourneyTestBase
{
    protected readonly PhaImportNotificationsClient PhaImportNotificationsClient =
        PhaImportNotificationsClient.Create();

    protected static async Task AssertSuccessStatusCode(HttpResponseMessage response, string context)
    {
        if (!response.IsSuccessStatusCode)
        {
            var responseBody = await response.Content.ReadAsStringAsync();
            response
                .IsSuccessStatusCode.Should()
                .BeTrue(
                    $"{context} failed with status code {response.StatusCode} ({(int)response.StatusCode}). Response body: {responseBody}"
                );
        }
    }
}
