namespace Tasker.Api.Features.Tasks.UpdateTask;

public sealed record UpdateTaskCommand(Guid Id, string Title, string? Description);
