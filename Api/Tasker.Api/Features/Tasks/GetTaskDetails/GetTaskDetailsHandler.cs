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
            .Where(x => x.Id == query.Id)
            .ProjectToType<GetTaskDetailsResponse>()
            .SingleOrDefaultAsync(cancellationToken);

        if (task == null)
            throw new NotFoundException(nameof(task), query.Id);

        return task;
    }
}
