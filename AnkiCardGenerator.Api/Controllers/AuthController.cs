using AnkiCardGenerator.Api.DTOs;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AnkiCardGenerator.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("google")]
        public async Task<IActionResult> Google([FromBody] GoogleAuthRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(request?.Credential))
                return BadRequest(new { error = "credential is required" });

            GoogleJsonWebSignature.Payload payload;
            try
            {
                // Validate the Google ID token. This will validate signature and expiry.
                payload = await GoogleJsonWebSignature.ValidateAsync(request.Credential);
            }
            catch
            {
                return Unauthorized(new { error = "invalid_token" });
            }

            var email = payload.Email ?? string.Empty;
            var name = payload.Name ?? string.Empty;

            var allowed = _configuration["ALLOWED_EMAILS"] ?? string.Empty;
            var allowedList = allowed.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(e => e.Trim()).Where(e => !string.IsNullOrEmpty(e)).ToList();

            if (!allowedList.Any() || !allowedList.Contains(email, StringComparer.OrdinalIgnoreCase))
            {
                return Unauthorized(new { error = "email_not_allowed" });
            }

            var jwtSecret = _configuration["JWT_SECRET"];
            if (string.IsNullOrEmpty(jwtSecret))
            {
                // For MVP we require JWT_SECRET to be configured
                return StatusCode(500, new { error = "server_misconfigured" });
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(jwtSecret!);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.NameIdentifier, payload.Subject ?? string.Empty),
                    new Claim(ClaimTypes.Email, email),
                    new Claim(ClaimTypes.Name, name)
                }),
                Expires = DateTime.UtcNow.AddHours(8),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new { token = tokenString, email, name });
        }
    }
}
