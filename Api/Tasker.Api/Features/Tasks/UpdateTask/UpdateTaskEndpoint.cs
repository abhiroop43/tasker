namespace Tasker.Api.Features.Tasks.UpdateTask;

public class UpdateTaskEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/tasks/{id:guid}", () => { });
    }
}
