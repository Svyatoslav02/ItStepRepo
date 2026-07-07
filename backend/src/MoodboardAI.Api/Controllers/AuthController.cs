using Microsoft.AspNetCore.Mvc;
using MoodboardAI.Api.DTOs.Auth;
using MoodboardAI.Api.Extensions;
using MoodboardAI.Api.Models;
using MoodboardAI.Api.Services;

namespace MoodboardAI.Api.Controllers;

/// <summary>
/// API controller that exposes authentication endpoints for user registration and login.
/// </summary>
[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthController"/> class.
    /// </summary>
    /// <param name="authService">Authentication service.</param>
    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// Registers a new user account with a full name, email, and password.
    /// </summary>
    /// <param name="request">Registration request.</param>
    /// <returns>JWT token and user data on success; 400 if the email is already registered or the request is invalid.</returns>
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState.ToErrorResponse());
        }

        var result = await _authService.RegisterAsync(request);

        if (!result.Succeeded)
        {
            return BadRequest(new ErrorResponse { Message = result.ErrorMessage ?? "Registration failed." });
        }

        return Ok(new AuthResponseDto
        {
            Token = result.Token!,
            User = result.User!
        });
    }

    /// <summary>
    /// Authenticates a user by email and password.
    /// </summary>
    /// <param name="request">Login request.</param>
    /// <returns>JWT token and user data on success; 401 if the credentials are invalid; 400 if the request is invalid.</returns>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState.ToErrorResponse());
        }

        var result = await _authService.LoginAsync(request);

        if (!result.Succeeded)
        {
            return Unauthorized(new ErrorResponse { Message = result.ErrorMessage ?? "Invalid email or password." });
        }

        return Ok(new AuthResponseDto
        {
            Token = result.Token!,
            User = result.User!
        });
    }
}
