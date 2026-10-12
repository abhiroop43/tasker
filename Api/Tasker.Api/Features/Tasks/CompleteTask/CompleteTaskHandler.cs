namespace Tasker.Api.Features.Tasks.CompleteTask;

public sealed class CompleteTaskHandler
{
    private CompleteTaskHandler() { }

    public static async Task Handle(
        CompleteTaskCommand command,
        ApplicationDbContext dbContext,
        CancellationToken cancellationToken
    )
    {
        var task = await dbContext.Tasks.FindAsync([command.Id], cancellationToken);

        if (task == null)
        {
            throw new NotFoundException(nameof(task), command.Id);
        }

        task.Complete();

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
