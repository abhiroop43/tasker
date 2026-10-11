namespace Tasker.Api.Features.Tasks.DeleteTask;

public sealed class DeleteTaskHandler
{
    private DeleteTaskHandler() { }

    public static async Task Handle(
        DeleteTaskCommand command,
        ApplicationDbContext dbContext,
        CancellationToken cancellationToken
    )
    {
        var task = await dbContext.Tasks.FindAsync([command.Id], cancellationToken);

        if (task == null)
            throw new NotFoundException(nameof(task), command.Id);

        dbContext.Tasks.Remove(task);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
