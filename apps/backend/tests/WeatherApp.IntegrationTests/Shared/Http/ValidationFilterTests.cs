using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using WeatherApp.Api.Shared.Http;

namespace WeatherApp.IntegrationTests.Shared.Http;

public sealed class ValidationFilterTests
{
    [Fact]
    public async Task InvokeAsync_WithoutRegisteredValidator_ThrowsClearException()
    {
        var services = new ServiceCollection().BuildServiceProvider();
        var httpContext = new DefaultHttpContext { RequestServices = services };
        var context = new TestEndpointFilterInvocationContext(
            httpContext,
            new TestRequest(48.85, 2.35)
        );
        var sut = new ValidationFilter<TestRequest>();

        ValueTask<object?> act() =>
            sut.InvokeAsync(context, _ => ValueTask.FromResult<object?>(TypedResults.Ok()));

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => act().AsTask());
        Assert.Contains(nameof(TestRequest), exception.Message, StringComparison.Ordinal);
    }

    private sealed record TestRequest(double Lat, double Lon);

    private sealed class TestEndpointFilterInvocationContext(
        HttpContext httpContext,
        params object?[] arguments
    ) : EndpointFilterInvocationContext
    {
        public override HttpContext HttpContext { get; } = httpContext;

        public override IList<object?> Arguments { get; } = arguments;

        public override T GetArgument<T>(int index) => (T)Arguments[index]!;
    }
}
