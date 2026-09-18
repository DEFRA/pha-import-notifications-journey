using System.Net.Http.Json;
using System.Text.Json;
using PhaImportNotifications.Tests.Models;

namespace PhaImportNotifications.Tests;

public class GetUpdatedImportNotificationsTests : JourneyTestBase
{
    [Fact]
    public async Task GetUpdatedImportNotificationsValid()
    {
        var response = await PhaImportNotificationsClient.GetUpdatedImportNotificationsAsync(
            "GBTEEP1",
            new DateTimeOffset(2026, 1, 1, 9, 0, 0, TimeSpan.Zero),
            new DateTimeOffset(2026, 1, 1, 9, 59, 0, TimeSpan.Zero),
            TestContext.Current.CancellationToken
        );

        await AssertSuccessStatusCode(response, "Getting updated import notifications");

        var content = await response.Content.ReadFromJsonAsync<UpdatedImportNotificationsResponse>(
            JsonSerializerOptions.Web,
            TestContext.Current.CancellationToken
        );

        await Verify(content);
    }
}
