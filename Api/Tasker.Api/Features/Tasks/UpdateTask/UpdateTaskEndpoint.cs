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
                    IValidator<UpdateTaskRequest> validator,
                    IMessageBus bus,
                    CancellationToken cancellationToken
                ) =>
                {
                    var requestWithId = request with { Id = id };
                    var validationResult = await validator.ValidateAsync(
                        requestWithId,
                        cancellationToken
                    );
                    if (!validationResult.IsValid)
                        throw new ValidationException(validationResult.Errors);

                    var command = requestWithId.Adapt<UpdateTaskCommand>();
                    await bus.InvokeAsync<UpdateTaskResponse>(command, cancellationToken);

                    return Results.NoContent();
                }
            )
            .WithName("UpdateTask")
            .WithSummary("Update Task")
            .WithDescription("Update an existing task")
            .Produces<UpdateTaskResponse>(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound);
    }
}
