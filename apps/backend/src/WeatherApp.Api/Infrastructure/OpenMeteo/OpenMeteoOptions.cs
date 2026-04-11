using System.ComponentModel.DataAnnotations;

namespace WeatherApp.Api.Infrastructure.OpenMeteo;

internal sealed class OpenMeteoOptions
{
    public const string SectionName = "OpenMeteo";

    [Required]
    public required string BaseUrl { get; init; }
}
