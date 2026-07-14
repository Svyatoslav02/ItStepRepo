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
        /// <summary>
        /// Gets the current user's ID from the JWT token claims.
        /// </summary>
        /// <returns>The user's ID.</returns>
        var userIdString = context.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

        // If the user ID is not present or is not a valid GUID, return an unauthorized result.
        if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        // Save the user ID in the HttpContext.Items for later use in the request processing pipeline.
        context.HttpContext.Items["UserId"] = userId;
    }
}

