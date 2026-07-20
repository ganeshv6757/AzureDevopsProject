
using System.Threading.Tasks;
using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Xunit;

namespace AzureWebApp.Tests;

public class WeatherForecastIntegrationTests : IClassFixture<Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactory<Program>>
{
    private readonly Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactory<Program> _factory;

    public WeatherForecastIntegrationTests(Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetWeatherForecast_ReturnsSuccessAndExpectedShape()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var resp = await client.GetAsync("/WeatherForecast");

        // Assert
        resp.StatusCode.Should().Be(HttpStatusCode.OK);
        var items = await resp.Content.ReadFromJsonAsync<WeatherForecast[]>();
        items.Should().NotBeNull();
        items!.Length.Should().BeGreaterThan(0);
        items[0].Should().BeOfType<WeatherForecast>();
    }

    [Fact]
    public async Task SwaggerUI_IsAvailable()
    {
        var client = _factory.CreateClient();
        var resp = await client.GetAsync("/swagger/index.html");
        resp.StatusCode.Should().Be(HttpStatusCode.OK);
        var html = await resp.Content.ReadAsStringAsync();
        html.Should().Contain("Swagger UI");
    }
}

// Minimal type to deserialize response (matches project model)
public class WeatherForecast
{
    public DateTime Date { get; set; }
    public int TemperatureC { get; set; }
    public string? Summary { get; set; }
}
