using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MoodboardAI.Api.Services;
using MoodboardAI.Api.DTOs.Auth;

namespace MoodboardAI.Api.Controllers
{
    /// <summary>
    /// Controller for handling user authentication: registration and login.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        /// <summary>
        /// In-memory list of users for demonstration purposes.
        /// In a real application, this would be replaced with 
        /// a database context for persistent storage.
        /// </summary>
        private static List<User> _users = new List<User>();
        /// <summary>
        /// Configuration instance for accessing app settings, including JWT configuration.
        /// </summary>
        private readonly IConfiguration _config;
        /// <summary>
        /// Password hasher service for securely hashing and verifying passwords using Argon2.
        /// </summary>
        private readonly PasswordHasher _passwordHasher = new PasswordHasher();

        public AuthController(IConfiguration config)
        {
            _config = config;
        }

        /// <summary>
        /// Registers a new user with the provided details and 
        /// returns a JWT token upon successful registration.
        /// </summary>
        /// <param name="request">The registration request containing user details.</param>
        /// <returns>The JWT token and user information upon successful registration.</returns>
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterRequestDto request)
        {
            if (_users.Any(u => u.Email == request.Email))
                return BadRequest("Email already exists");

            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                FullName = request.UserName,
                Email = request.Email
            };

            // Hash the password using Argon2 and store the hash
            user.PasswordHash = _passwordHasher.HashPassword(request.Password);

            _users.Add(user);

            var token = GenerateJwtToken(user);
            return Ok(new { token, user });
        }

        /// <summary>
        /// Logs in a user with the provided credentials and returns 
        /// a JWT token upon successful login.
        /// </summary>
        /// <param name="request">The login request containing user credentials.</param>
        /// <returns>The JWT token and user information upon successful login.</returns>
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequestDto request)
        {
            var user = _users.FirstOrDefault(u => u.Email == request.Email);
            if (user == null)
                return Unauthorized("Invalid credentials");

            if (!_passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
                return Unauthorized("Invalid credentials");

            var token = GenerateJwtToken(user);
            return Ok(new { token, user });
        }

        /// <summary>
        /// Generates a JWT token for the specified user, including claims 
        /// for user ID, email, and full name.
        /// </summary>
        /// <param name="user">The user for whom to generate a JWT token.</param>
        /// <returns>The generated JWT token.</returns>
        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("fullName", user.FullName)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

