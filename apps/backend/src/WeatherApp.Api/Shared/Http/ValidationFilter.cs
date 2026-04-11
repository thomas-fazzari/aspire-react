using FluentValidation;

namespace WeatherApp.Api.Shared.Http;

/// <summary>
/// Validates a bound endpoint argument of type <typeparamref name="T"/> before invoking the handler.
/// </summary>
internal sealed class ValidationFilter<T> : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(
        EndpointFilterInvocationContext ctx,
        EndpointFilterDelegate next
    )
    {
        var arg = ctx.Arguments.OfType<T>().FirstOrDefault();

        if (arg is null)
            return await next(ctx);

        var validator =
            ctx.HttpContext.RequestServices.GetService<IValidator<T>>()
            ?? throw new InvalidOperationException(
                $"Validation filter for '{typeof(T).Name}' requires a registered IValidator<{typeof(T).Name}>."
            );

        var result = await validator.ValidateAsync(arg, ctx.HttpContext.RequestAborted);
        if (!result.IsValid)
            return TypedResults.ValidationProblem(result.ToDictionary());

        return await next(ctx);
    }
}

internal static class ValidationFilterExtensions
{
    /// <summary>
    /// Registers a validation filter that resolves <see cref="IValidator{T}"/> from DI.
    /// </summary>
    public static RouteHandlerBuilder WithValidation<T>(this RouteHandlerBuilder builder) =>
        builder.AddEndpointFilter<ValidationFilter<T>>();
}
