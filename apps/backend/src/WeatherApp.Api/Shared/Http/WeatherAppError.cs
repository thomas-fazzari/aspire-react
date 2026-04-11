namespace WeatherApp.Api.Shared.Http;

internal enum WeatherAppErrorType
{
    Validation,
    NotFound,
    Conflict,
    Unauthorized,
    Forbidden,
    BusinessRule,
    Failure,
}

internal sealed class WeatherAppError(WeatherAppErrorType type, string code, string message)
    : Error(message)
{
    public WeatherAppErrorType Type { get; } = type;
    public string Code { get; } = code;

    public static WeatherAppError NotFound(string code, string message) =>
        new(WeatherAppErrorType.NotFound, code, message);

    public static WeatherAppError Failure(string code, string message) =>
        new(WeatherAppErrorType.Failure, code, message);

    public static WeatherAppError BusinessRule(string code, string message) =>
        new(WeatherAppErrorType.BusinessRule, code, message);
}
