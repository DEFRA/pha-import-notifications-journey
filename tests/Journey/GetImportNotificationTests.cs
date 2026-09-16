using System.Net.Http.Json;
using System.Text.Json;
using PhaImportNotifications.Tests.Models;

namespace PhaImportNotifications.Tests;

public class GetImportNotificationTests : JourneyTestBase
{
    [Fact]
    public async Task GetImportNotificationValid()
    {
        var response = await PhaImportNotificationsClient.GetImportNotificationAsync(
            "CHEDA.GB.2024.4792831",
            TestContext.Current.CancellationToken
        );

        await AssertSuccessStatusCode(response, "Getting import notification");

        var content = await response.Content.ReadFromJsonAsync<ImportNotificationResponse>(
            JsonSerializerOptions.Web,
            TestContext.Current.CancellationToken
        );

        await Verify(content);
    }
}
