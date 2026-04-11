using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;

namespace WeatherApp.TestHelpers.Mocks;

/// <summary>
/// Registers mocks in the DI container after removing the existing service registration
/// </summary>
internal static class ServiceCollectionMockExtensions
{
    /// <summary>
    /// Replaces an existing service registration with the supplied mock instance
    /// </summary>
    internal static IServiceCollection AddMock<TService>(
        this IServiceCollection services,
        Mock<TService> mock
    )
        where TService : class
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(mock);

        services.RemoveAll<TService>();
        services.AddSingleton(mock.Object);

        return services;
    }
}
