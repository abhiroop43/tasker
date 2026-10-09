namespace Tasker.Api.Features.Tasks.CreateTask;

public sealed record CreateTaskRequest(string Title, string? Description);

public sealed record CreateTaskResponse(
    Guid Id,
    string Title,
    string? Description,
    bool IsCompleted,
    DateTime CreatedAt
);

public class CreateTaskEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost(
                "/api/tasks",
                async (
                    CreateTaskRequest request,
                    IValidator<CreateTaskRequest> validator,
                    IMessageBus bus,
                    CancellationToken cancellationToken
                ) =>
                {
                    var validationResult = await validator.ValidateAsync(
                        request,
                        cancellationToken
                    );
                    if (!validationResult.IsValid)
                        return Results.ValidationProblem(validationResult.ToDictionary());

                    var command = new CreateTaskCommand(request.Title, request.Description);
                    var response = await bus.InvokeAsync<CreateTaskResponse>(
                        command,
                        cancellationToken
                    );

                    return Results.Created($"/api/tasks/{response.Id}", response);
                }
            )
            .WithName("CreateTask")
            .WithSummary("Create Task")
            .WithDescription("Creates a new task")
            .Produces<CreateTaskResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest);
    }
}
