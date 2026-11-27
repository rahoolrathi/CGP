using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace CgbSteps.Tests.Api;

public sealed class ApiTests(ApiFactory factory) : IClassFixture<ApiFactory>
{
    private readonly HttpClient client = factory.CreateClient();
    private static CancellationToken Ct => TestContext.Current.CancellationToken;

    [Fact]
    public async Task Liveness_IsPublicAndHealthy()
    {
        var response = await client.GetAsync(new Uri("/health/live", UriKind.Relative), Ct);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task SystemInfo_IsPublicAndReturnsContract()
    {
        var json = await client.GetFromJsonAsync<JsonElement>(new Uri("/api/v1/system/info", UriKind.Relative), Ct);

        Assert.False(string.IsNullOrEmpty(json.GetProperty("version").GetString()));
        Assert.True(json.TryGetProperty("serverTimeUtc", out _));
        Assert.Matches(@"^\d{4}-\d{2}-\d{2}$", json.GetProperty("qatarDate").GetString());
    }

    [Fact]
    public async Task UnmarkedRoutes_AreDeniedByDefaultWithProblemDetails()
    {
        var response = await client.GetAsync(new Uri("/api/v1/does-not-exist", UriKind.Relative), Ct);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
        var problem = await response.Content.ReadFromJsonAsync<JsonElement>(Ct);
        Assert.Equal("unauthorized", problem.GetProperty("code").GetString());
    }

    [Fact]
    public async Task Responses_IncludeSecurityHeaders()
    {
        var response = await client.GetAsync(new Uri("/health/live", UriKind.Relative), Ct);

        Assert.Equal("nosniff", response.Headers.GetValues("X-Content-Type-Options").Single());
        Assert.Equal("DENY", response.Headers.GetValues("X-Frame-Options").Single());
        Assert.Contains("default-src 'none'", response.Headers.GetValues("Content-Security-Policy").Single(), StringComparison.Ordinal);
    }

    [Fact]
    public async Task Cors_AllowsOnlyConfiguredOrigins()
    {
        var allowed = await SendPreflight("https://admin.example.test");
        var blocked = await SendPreflight("https://evil.example.test");

        Assert.Equal("https://admin.example.test", allowed.Headers.GetValues("Access-Control-Allow-Origin").Single());
        Assert.False(blocked.Headers.Contains("Access-Control-Allow-Origin"));
    }

    [Fact]
    public async Task OpenApi_IsNotExposedOutsideDevelopment()
    {
        var response = await client.GetAsync(new Uri("/openapi/v1.json", UriKind.Relative), Ct);

        Assert.NotEqual(HttpStatusCode.OK, response.StatusCode);
    }

    private async Task<HttpResponseMessage> SendPreflight(string origin)
    {
        using var request = new HttpRequestMessage(HttpMethod.Options, "/api/v1/system/info");
        request.Headers.Add("Origin", origin);
        request.Headers.Add("Access-Control-Request-Method", "GET");
        return await client.SendAsync(request, Ct);
    }
}
