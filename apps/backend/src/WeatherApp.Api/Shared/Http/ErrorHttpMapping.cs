namespace WeatherApp.Api.Shared.Http;

internal static class ErrorHttpMapping
{
    public static ProblemDetails ToProblem(this WeatherAppError error)
    {
        var statusCode = error.Type switch
        {
            WeatherAppErrorType.Validation => StatusCodes.Status400BadRequest,
            WeatherAppErrorType.NotFound => StatusCodes.Status404NotFound,
            WeatherAppErrorType.Conflict => StatusCodes.Status409Conflict,
            WeatherAppErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            WeatherAppErrorType.Forbidden => StatusCodes.Status403Forbidden,
            WeatherAppErrorType.BusinessRule => StatusCodes.Status422UnprocessableEntity,
            _ => StatusCodes.Status500InternalServerError,
        };

        return new ProblemDetails
        {
            Title = error.Type.ToString(),
            Detail = error.Message,
            Status = statusCode,
            Extensions = { ["code"] = error.Code },
        };
    }
}
