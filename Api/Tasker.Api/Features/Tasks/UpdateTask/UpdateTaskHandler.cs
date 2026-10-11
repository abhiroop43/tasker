namespace Tasker.Api.Features.Tasks.UpdateTask;

public sealed class UpdateTaskHandler
{
    private UpdateTaskHandler() { }

    public static async Task<UpdateTaskResponse> Handle(
        UpdateTaskCommand command,
        ApplicationDbContext dbContext,
        CancellationToken cancellationToken
    )
    {
        var currentTask = await dbContext.Tasks.FindAsync([command.Id], cancellationToken);

        if (currentTask == null)
            throw new NotFoundException(nameof(currentTask), command.Id);

        currentTask.Update(command.Title, command.Description);

        await dbContext.SaveChangesAsync(cancellationToken);

        return new UpdateTaskResponse(true);
    }
}
