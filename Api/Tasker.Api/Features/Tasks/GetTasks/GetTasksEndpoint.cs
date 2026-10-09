namespace Tasker.Api.Features.Tasks.GetTasks;

public class GetTasksEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/tasks", () => { });
    }
}
