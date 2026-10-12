namespace Tasker.Api.Features.Tasks.GetTasks;

public sealed record GetTasksQuery(
    PaginationRequest PaginationRequest,
    string? Search = null,
    bool? IsCompleted = null
);
