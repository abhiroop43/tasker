namespace Tasker.Api.Infrastructure.Validation;

public sealed class ValidationFilter<T> : IEndpointFilter
    where T : notnull
{
    public async ValueTask<object?> InvokeAsync(
        EndpointFilterInvocationContext context,
        EndpointFilterDelegate next
    )
    {
        var validator = context.HttpContext.RequestServices.GetService<IValidator<T>>();

        if (validator is null)
        {
            return await next(context);
        }

        var request = context.Arguments.OfType<T>().FirstOrDefault();

        if (request is null)
        {
            return await next(context);
        }

        var validationResult = await validator.ValidateAsync(
            request,
            context.HttpContext.RequestAborted
        );

        if (!validationResult.IsValid)
        {
            var errors = validationResult
                .Errors.GroupBy(error => error.PropertyName)
                .ToDictionary(
                    group => group.Key,
                    group => group.Select(error => error.ErrorMessage).ToArray()
                );

            return Results.ValidationProblem(errors);
        }

        return await next(context);
    }
}
