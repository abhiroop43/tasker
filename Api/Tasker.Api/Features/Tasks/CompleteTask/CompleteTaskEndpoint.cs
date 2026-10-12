namespace Tasker.Api.Features.Tasks.CompleteTask;

public class CompleteTaskEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPatch(
                "/api/tasks/{id:guid}/complete",
                async ([FromRoute] Guid id, IMessageBus bus, CancellationToken cancellationToken) =>
                {
                    var command = new CompleteTaskCommand(id);
                    await bus.InvokeAsync(command, cancellationToken);

                    return Results.NoContent();
                }
            )
            .WithName("CompleteTask")
            .WithSummary("Complete Task")
            .WithDescription("Mark a task as completed")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound);
    }
}
