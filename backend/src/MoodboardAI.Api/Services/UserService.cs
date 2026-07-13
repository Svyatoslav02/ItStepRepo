namespace MoodboardAI.Api.Services;

public class UserService : IUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid GetCurrentUserId()
    {
        var userId = _httpContextAccessor.HttpContext?.User
            .FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        if (Guid.TryParse(userId, out Guid parsedUserId))
        {
            return parsedUserId;
        }

        throw new InvalidOperationException("Unable to parse user ID.");
    }
}
