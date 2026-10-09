namespace Tasker.Api.Features.Tasks.GetTaskDetails;

public class GetTaskDetailsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/tasks/{id:guid}", () => { });
    }
}
