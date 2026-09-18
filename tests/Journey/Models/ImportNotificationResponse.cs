namespace PhaImportNotifications.Tests.Models;

public sealed record ImportNotificationResponse
{
    public string? ReferenceNumber { get; init; }

    public string? ImportNotificationType { get; init; }

    public string? Status { get; init; }

    public PartOne? PartOne { get; init; }
}

public sealed record PartOne
{
    public string? PointOfEntry { get; init; }
}
