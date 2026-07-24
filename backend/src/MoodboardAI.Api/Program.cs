using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MoodboardAI.Api.Configuration;
using MoodboardAI.Api.Data;
using MoodboardAI.Api.Services;

// Load variables from a .env file (if one exists anywhere above the current
// working directory) into the process environment before configuration is
// built. This lets every developer keep their own local secrets (DB
// connection string, JWT secret, API keys) in a git-ignored .env file at the
// repo root, per docs/database-setup.md and docs/environment.md. Variables
// that are already set (real OS/CI environment variables) always win.
LoadDotEnvFile();

var builder = WebApplication.CreateBuilder(args);

// Connect to PostgreSQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddScoped<IMoodboardService, MockMoodboardService>();

// Use our own ErrorResponse shape for invalid model state instead of the
// default ASP.NET Core ProblemDetails response.
builder.Services.Configure<Microsoft.AspNetCore.Mvc.ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

// Database (Supabase PostgreSQL via Npgsql).
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IMoodboardService, MockMoodboardService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IInterestsService, InterestsService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserService, MockUserService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUserService, UserService>();


// JWT settings from configuration
var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtSettings>() ?? new JwtSettings();
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));

// JWT token service
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

// JWT bearer authentication. Required for [Authorize]-protected endpoints
// (e.g. POST /api/users/me/interests) to validate the tokens issued by
// IJwtTokenService and reject missing/invalid/expired tokens with 401.
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.MapInboundClaims = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidateAudience = true,
            ValidAudience = jwtSettings.Audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };

        // Ensure 401/403 responses use the standard ErrorResponse shape
        // instead of ASP.NET Core's default empty body.
        options.Events = new JwtBearerEvents
        {
            OnChallenge = async context =>
            {
                context.HandleResponse();
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(new MoodboardAI.Api.Models.ErrorResponse
                {
                    Message = "Authentication is required to access this resource."
                });
            },
            OnForbidden = async context =>
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(new MoodboardAI.Api.Models.ErrorResponse
                {
                    Message = "You do not have permission to access this resource."
                });
            }
        };
    });

builder.Services.AddAuthorization();

// Use our own ErrorResponse shape for invalid model state instead of the
// default ASP.NET Core ProblemDetails response.
builder.Services.Configure<Microsoft.AspNetCore.Mvc.ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

// Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "MoodboardAI API",
        Version = "v1"
    });

    // Lets Swagger UI send a "Bearer <token>" Authorization header, so
    // [Authorize]-protected endpoints (e.g. POST /api/users/me/interests)
    // can be exercised directly from the docs.
    var bearerScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter the JWT token returned by /api/auth/login or /api/auth/register.",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };

    options.AddSecurityDefinition("Bearer", bearerScheme);
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { bearerScheme, Array.Empty<string>() }
    });
});

var app = builder.Build();

// Automatically apply any pending EF Core migrations against the configured
// database when running locally. This means every teammate only has to
// `git pull` and `dotnet run` to get an up-to-date local schema, without
// remembering to run `dotnet ef database update` by hand every time. See
// docs/database-setup.md. Other environments (Staging/Production) apply
// migrations explicitly as part of deployment instead.
if (app.Environment.IsDevelopment())
{
    using var migrationScope = app.Services.CreateScope();
    var dbContext = migrationScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();
}

// Catches unhandled exceptions from everything below and turns them into a
// standardized ErrorResponse JSON body (500) instead of the ASP.NET Core
// default. Registered first so it wraps the entire remaining pipeline.
app.UseMiddleware<MoodboardAI.Api.Middleware.ExceptionHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "MoodboardAI API v1");
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Standardized 404 for any route that doesn't match a controller action,
// so unmatched routes return the same ErrorResponse shape as other errors
// instead of an empty body.
app.MapFallback(context =>
{
    context.Response.StatusCode = StatusCodes.Status404NotFound;
    context.Response.ContentType = "application/json";
    return context.Response.WriteAsJsonAsync(new MoodboardAI.Api.Models.ErrorResponse
    {
        Message = "The requested resource was not found."
    });
});

app.Run();

/// <summary>
/// Searches the current working directory and its parents for a ".env"
/// file and, if found, loads any "KEY=VALUE" lines into the process
/// environment (skipping keys that are already set, so real environment
/// variables always take priority over the .env file). Silently does
/// nothing if no .env file is found, which is expected in CI/production
/// where secrets are provided as real environment variables instead.
/// </summary>
static void LoadDotEnvFile()
{
    var directory = new DirectoryInfo(Directory.GetCurrentDirectory());

    while (directory is not null)
    {
        var candidatePath = Path.Combine(directory.FullName, ".env");

        if (File.Exists(candidatePath))
        {
            foreach (var rawLine in File.ReadAllLines(candidatePath))
            {
                var line = rawLine.Trim();

                if (line.Length == 0 || line.StartsWith('#'))
                {
                    continue;
                }

                var separatorIndex = line.IndexOf('=');

                if (separatorIndex <= 0)
                {
                    continue;
                }

                var key = line[..separatorIndex].Trim();
                var value = line[(separatorIndex + 1)..].Trim().Trim('"');

                if (Environment.GetEnvironmentVariable(key) is null)
                {
                    Environment.SetEnvironmentVariable(key, value);
                }
            }

            return;
        }

        directory = directory.Parent;
    }
}