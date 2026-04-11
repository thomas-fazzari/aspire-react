namespace WeatherApp.Api.Shared.Http;

internal static class ResultExtensions
{
    public static Results<Ok<T>, ProblemHttpResult> ToHttpResult<T>(this Result<T> result) =>
        result.IsSuccess
            ? TypedResults.Ok(result.Value)
            : TypedResults.Problem(result.Errors.OfType<WeatherAppError>().First().ToProblem());
}
