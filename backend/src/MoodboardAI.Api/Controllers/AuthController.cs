using Microsoft.AspNetCore.Mvc;
using MoodboardAI.Api.DTOs.Auth;
using MoodboardAI.Api.Models;
using MoodboardAI.Api.Services;

namespace MoodboardAI.Api.Controllers;
[ApiController] 
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register(RegisterRequestDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ErrorHelper.Create(400, "Validation error"));

        try
        {
            var result = await _authService.RegisterAsync(dto);
            return Ok(result);
        }
        catch (Exception ex) when (ex.Message.Contains("already exists"))
        {
            return BadRequest(ErrorHelper.Create(400, "Email already exists"));
        }
        catch (Exception ex)
        {
            return BadRequest(ErrorHelper.Create(400, ex.Message));
        }
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login(LoginRequestDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ErrorHelper.Create(400, "Validation error"));

        try
        {
            var result = await _authService.LoginAsync(dto);
            return Ok(result);
        }
        catch (Exception ex) when (ex.Message.Contains("invalid") || ex.Message.Contains("does not exist"))
        {
            return Unauthorized(ErrorHelper.Create(401, "Invalid credentials"));
        }
        catch (Exception ex)
        {
            return BadRequest(ErrorHelper.Create(400, ex.Message));
        }
    }

}