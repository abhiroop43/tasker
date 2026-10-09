namespace Tasker.Api.Features.Tasks.DeleteTask;

public class DeleteTaskEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/tasks/{id:guid}", () => { });
    }
}
