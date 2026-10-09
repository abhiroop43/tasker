namespace Tasker.Api.Features.Tasks.GetTaskDetails;

public sealed class GetTaskDetailsHandler
{
    private GetTaskDetailsHandler() { }

    public static async Task<GetTaskDetailsResponse> Handle(
        GetTaskDetailsQuery query,
        ApplicationDbContext dbContext,
        CancellationToken cancellationToken
    )
    {
        var task = await dbContext
            .Tasks.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == query.Id, cancellationToken);

        if (task == null)
            throw new NotFoundException(nameof(task), query.Id);

        return new GetTaskDetailsResponse(
            task.Id,
            task.Title,
            task.Description,
            task.IsCompleted,
            task.CreatedAt
        );
    }
}
