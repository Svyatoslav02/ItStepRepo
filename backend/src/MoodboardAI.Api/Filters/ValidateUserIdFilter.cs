using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace MoodboardAI.Api.Filters;

/// <summary>
/// An authorization filter that validates the user ID from the JWT token claims.
/// </summary>
public class ValidateUserIdFilter : IAuthorizationFilter
{
    /// <summary>
    /// Called during the authorization phase of the request 
    /// processing pipeline to validate the user ID from the JWT token claims.
    /// </summary>
    /// <param name="context">The authorization filter context.</param>
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        // Only endpoints that actually require authorization (i.e. carry an
        // [Authorize] attribute and are not [AllowAnonymous]) need a
        // validated user ID. This keeps genuinely public endpoints (e.g.
        // AuthController.Login/Register, MoodboardController.Generate)
        // working, and automatically covers any future public endpoint
        // without needing to hardcode controller names here.
        var endpoint = context.HttpContext.GetEndpoint();

        if (endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null)
        {
            return;
        }

        if (endpoint?.Metadata?.GetMetadata<IAuthorizeData>() == null)
        {
            return;
        }

        var userIdString = context.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

        // If the user ID is missing or invalid, return an unauthorized response
        if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        // Store the validated user ID in the HttpContext.Items for later use in the request pipeline
        context.HttpContext.Items["UserId"] = userId;
    }
}