namespace Tasker.Api.Features.Tasks.GetTasks;

public sealed class GetTasksHandler
{
    private GetTasksHandler() { }

    public static async Task<GetTasksResponse> Handle(
        GetTasksQuery query,
        ApplicationDbContext dbContext,
        CancellationToken cancellationToken
    )
    {
        const int minSearchLength = 3;

        var searchTerm = query.Search?.Trim();

        if (searchTerm?.Length < minSearchLength)
        {
            searchTerm = null;
        }

        var tasksQuery = dbContext.Tasks.AsNoTracking().AsQueryable();

        if (!string.IsNullOrEmpty(searchTerm))
        {
            tasksQuery = tasksQuery.Where(task =>
                EF.Functions.ILike(task.Title, $"%{searchTerm}%")
                || (
                    task.Description != null
                    && EF.Functions.ILike(task.Description, $"%{searchTerm}%")
                )
            );
        }

        if (query.IsCompleted.HasValue)
        {
            tasksQuery = tasksQuery.Where(task => task.IsCompleted == query.IsCompleted.Value);
        }

        var totalCount = await tasksQuery.LongCountAsync(cancellationToken);

        var tasks = await tasksQuery
            .OrderByDescending(x => x.CreatedAt)
            .ThenBy(x => x.Id)
            .Skip(query.PaginationRequest.PageIndex * query.PaginationRequest.PageSize)
            .Take(query.PaginationRequest.PageSize)
            .ProjectToType<TaskListItemDto>()
            .ToListAsync(cancellationToken);

        var result = new PaginationResult<TaskListItemDto>(
            query.PaginationRequest.PageIndex + 1,
            query.PaginationRequest.PageSize,
            totalCount,
            tasks
        );

        return new GetTasksResponse(result);
    }
}
