using Tasker.Api.Infrastructure.Validation;

namespace Tasker.Api.Features.Tasks.UpdateTask;

public sealed record UpdateTaskRequest(Guid Id, string Title, string? Description);

public sealed record UpdateTaskResponse(bool Success);

public class UpdateTaskEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut(
                "/api/tasks/{id:guid}",
                async (
                    [FromRoute] Guid id,
                    [FromBody] UpdateTaskRequest request,
                    IMessageBus bus,
                    CancellationToken cancellationToken
                ) =>
                {
                    var requestWithId = request with { Id = id };

                    var command = requestWithId.Adapt<UpdateTaskCommand>();
                    await bus.InvokeAsync<UpdateTaskResponse>(command, cancellationToken);

                    return Results.NoContent();
                }
            )
            .AddEndpointFilter<ValidationFilter<UpdateTaskRequest>>()
            .WithName("UpdateTask")
            .WithSummary("Update Task")
            .WithDescription("Update an existing task")
            .Produces<UpdateTaskResponse>(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound);
    }
}
