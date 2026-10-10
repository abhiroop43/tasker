namespace Tasker.Api.Dtos;

public sealed record TaskListItemDto(
    Guid Id,
    string Title,
    string? Description,
    bool IsCompleted,
    DateTime CreatedAt
);
