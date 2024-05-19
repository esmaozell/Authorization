using Login_Register.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Login_Register.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AuthenticationController : Controller
    {
        private readonly JwtSettings _jwtSettings;
        public AuthenticationController(IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }

        [AllowAnonymous]
        [HttpPost("/login")]
        public IActionResult Login(User userInformation)
        {
            var user = Authontication(userInformation);
            if (user == null) return NotFound("User not found");

            var token = CreateToken(user);

            return Ok(token);
        }

        private string CreateToken(User user)
        {

            if (_jwtSettings.Key == null) throw new Exception("Key can not be null");

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserName!),
                new Claim(ClaimTypes.Role, user.Role!)
            };

            var token = new JwtSecurityToken(_jwtSettings.Issuer,
                _jwtSettings.Audience,
                claims,
                expires: DateTime.Now.AddDays(15),
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
         
        }

        private User? Authontication(User user)
        {
            return ApiUsers
                .Users
                .FirstOrDefault(x =>
                    x.UserName?.ToLower() == user.UserName.ToLower() &&
                    x.Password == user.Password);
        }
    }
}
