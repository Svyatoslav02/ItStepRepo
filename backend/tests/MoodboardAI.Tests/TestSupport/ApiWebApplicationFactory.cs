using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MoodboardAI.Api.Data;

namespace MoodboardAI.Tests.TestSupport;

/// <summary>
/// Shared <see cref="WebApplicationFactory{TEntryPoint}"/> for HTTP-level
/// integration tests. Swaps the real PostgreSQL provider for a fresh,
/// isolated in-memory database per factory instance, and runs under the
/// "Testing" environment (see <c>Program.cs</c>) so no real database
/// connection or migration is required in CI.
/// </summary>
/// <remarks>
/// Mirrors the pattern already used by <c>NotificationIntegrationTests</c>,
/// extracted here so other controller test suites (Users, Privacy, Pins,
/// RecentSearches, Feed, ...) can reuse it instead of duplicating the
/// in-memory DB wiring and test-JWT signing logic.
/// </remarks>
public class ApiWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly string _databaseName = $"ApiTests-{Guid.NewGuid()}";

    // Must match the "Jwt" section in appsettings.json so tokens signed here
    // validate successfully against the running app (Testing environment
    // still reads the base appsettings.json — see Program.cs JWT setup).
    private const string TestSecretKey = "change-this-secret-key-in-production-min32chars";
    private const string TestIssuer = "MoodboardAI";
    private const string TestAudience = "MoodboardAI";

    /// <inheritdoc />
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase(_databaseName));
        });
    }

    /// <summary>
    /// Opens a DI scope backed by the same in-memory database the HTTP
    /// pipeline uses, for seeding data before a request and asserting
    /// persisted state afterwards.
    /// </summary>
    public IServiceScope CreateDbScope() => Services.CreateScope();

    /// <summary>
    /// Creates an <see cref="HttpClient"/> with no Authorization header, for
    /// exercising anonymous endpoints or verifying that protected endpoints
    /// reject unauthenticated requests.
    /// </summary>
    public HttpClient CreateAnonymousClient() => CreateClient();

    /// <summary>
    /// Creates an <see cref="HttpClient"/> authenticated as the given user
    /// via a signed test JWT (same shape the real <c>JwtTokenService</c> issues).
    /// </summary>
    public HttpClient CreateAuthenticatedClient(Guid userId, string email = "test@example.com")
    {
        var client = CreateClient();
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", CreateTestJwt(userId, email));
        return client;
    }

    private static string CreateTestJwt(Guid userId, string email)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(TestSecretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: TestIssuer,
            audience: TestAudience,
            claims:
            [
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            ],
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
