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
        var totalCount = await dbContext.Tasks.AsNoTracking().LongCountAsync(cancellationToken);

        var tasks = await dbContext
            .Tasks.AsNoTracking()
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
