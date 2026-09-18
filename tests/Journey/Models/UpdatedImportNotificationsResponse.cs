namespace PhaImportNotifications.Tests.Models;

public sealed record UpdatedImportNotificationsResponse
{
    public IReadOnlyList<UpdatedImportNotification> ImportNotifications { get; init; } = [];

    public PagingMetadata Paging { get; init; } = new();
}

public sealed record UpdatedImportNotification
{
    public DateTimeOffset Updated { get; init; }

    public string? ReferenceNumber { get; init; }

    public UpdatedImportNotificationLinks Links { get; init; } = new();
}

public sealed record UpdatedImportNotificationLinks
{
    public string? ImportNotification { get; init; }
}

public sealed record PagingMetadata
{
    public int Page { get; init; }

    public int PageSize { get; init; }

    public int Total { get; init; }
}
