namespace Tasker.Api.Features.Tasks.GetTaskDetails;

public sealed record GetTaskDetailsResponse(
    Guid Id,
    string Title,
    string? Description,
    bool IsCompleted,
    DateTime CreatedAt
);

public class GetTaskDetailsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(
                "/api/tasks/{id:guid}",
                async ([FromRoute] Guid id, IMessageBus bus, CancellationToken cancellationToken) =>
                {
                    var query = new GetTaskDetailsQuery(id);

                    var response = await bus.InvokeAsync<GetTaskDetailsResponse>(
                        query,
                        cancellationToken
                    );

                    return Results.Ok(response);
                }
            )
            .WithName("GetTaskDetails")
            .WithSummary("Get Task Details")
            .WithDescription("Get Task details by Id")
            .Produces<GetTaskDetailsResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound);
    }
}
