namespace Tasker.Api.Features.Tasks.CreateTask;

public sealed class CreateTaskHandler
{
    private CreateTaskHandler() { }

    public static async Task<CreateTaskResponse> Handle(
        CreateTaskCommand command,
        ApplicationDbContext dbContext,
        CancellationToken cancellationToken
    )
    {
        var task = new TaskItem(command.Title, command.Description);
        dbContext.Tasks.Add(task);
        await dbContext.SaveChangesAsync(cancellationToken);
        return new CreateTaskResponse(
            task.Id,
            task.Title,
            task.Description,
            task.IsCompleted,
            task.CreatedAt
        );
    }
}
