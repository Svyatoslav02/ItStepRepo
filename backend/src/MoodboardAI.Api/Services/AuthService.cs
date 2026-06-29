using Microsoft.AspNetCore.Identity;
using MoodboardAI.Api.DTOs.Auth;

namespace MoodboardAI.Api.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly IConfiguration _configuration;
    
    public AuthService(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager ,IConfiguration configuration)
    {
        _userManager = userManager;
        _configuration = configuration;
        _signInManager = signInManager;
    }


    public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto userDto)
    {
        var existingUser = await _userManager.FindByEmailAsync(userDto.Email);
        if (existingUser != null)
        {
            throw new Exception("User with this email already exists.");
        }

        var user = new IdentityUser
        {
            UserName = userDto.Email,
            Email = userDto.Email
        };
        var result = await _userManager.CreateAsync(user, userDto.Password);
        if (!result.Succeeded)
        {
            throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
        }
        var token = GenerateToken(user);//Name by generate Token
        return new AuthResponseDto
        {
            Token = token,
            User = new UserDto
            {
                Id = user.Id,
                FullName = userDto.FullName,
                Email = userDto.Email
            }
        };
    }

    public async Task<AuthResponseDto> LoginAsync(LoginRequestDto userDto)
    {
        var existingUser = await _userManager.FindByEmailAsync(userDto.Email);
        if (existingUser == null)
        {
            throw new Exception("User with this email does not exist.");
        }
        var result = await _signInManager.CheckPasswordSignInAsync(existingUser, userDto.Password, false);
        if (!result.Succeeded)
        {
            throw new Exception("This password is invalid.");
        }
        var token = GenerateToken(existingUser);//Name by generate Token

        return new AuthResponseDto
        {
            Token = token,
            User = new UserDto
            {
                Id = existingUser.Id,
                Email = userDto.Email
            }
        };
    }
}