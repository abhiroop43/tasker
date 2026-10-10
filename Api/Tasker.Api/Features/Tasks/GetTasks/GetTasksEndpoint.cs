namespace Tasker.Api.Features.Tasks.GetTasks;

public sealed record GetTasksResponse(PaginationResult<TaskListItemDto> Items);

public class GetTasksEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(
                "/api/tasks",
                async (
                    [FromQuery] int? pageNumber,
                    [FromQuery] int? pageSize,
                    IMessageBus bus,
                    CancellationToken cancellationToken
                ) =>
                {
                    var normalizedPageNumber = Math.Max(pageNumber ?? 1, 1);
                    var normalizedPageSize = Math.Clamp(pageSize ?? 10, 1, 100);

                    var query = new GetTasksQuery(
                        new PaginationRequest(normalizedPageNumber - 1, normalizedPageSize)
                    );

                    var response = await bus.InvokeAsync<GetTasksResponse>(
                        query,
                        cancellationToken
                    );

                    return Results.Ok(response);
                }
            )
            .WithName("GetTasks")
            .WithSummary("Get Tasks")
            .WithDescription("Get a paginated list of tasks")
            .Produces<GetTasksResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound);
    }
}
