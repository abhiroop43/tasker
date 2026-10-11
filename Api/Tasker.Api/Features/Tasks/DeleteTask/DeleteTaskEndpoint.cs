namespace Tasker.Api.Features.Tasks.DeleteTask;

public class DeleteTaskEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete(
                "/api/tasks/{id:guid}",
                async ([FromRoute] Guid id, IMessageBus bus, CancellationToken cancellationToken) =>
                {
                    var command = new DeleteTaskCommand(id);

                    await bus.InvokeAsync(command, cancellationToken);

                    return Results.NoContent();
                }
            )
            .WithName("DeleteTask")
            .WithSummary("Delete Task")
            .WithDescription("Delete an existing task using the Id")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound);
    }
}
