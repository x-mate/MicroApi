using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MicroApi.Demo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("allow_all")]
    public class LoginController : ControllerBase
    {
        private JwtSettings _jwtSeetings;
        private ILogger<LoginController> _logger;
        public LoginController(IOptions<JwtSettings> option, ILogger<LoginController> logger)
        {
            _jwtSeetings = option.Value;
            _logger = logger;
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            try
            {
                var claims = new Claim[]
                   {
                    new Claim(ClaimTypes.Name,username),
                    new Claim(ClaimTypes.Role,"admin")
                   };
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSeetings.SecretKey));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    _jwtSeetings.Issuer,
                    _jwtSeetings.Audience,
                    claims,
                    DateTime.Now,
                    DateTime.Now.AddMinutes(30),
                    creds
                    );
                return Ok(new { success = true, data = new JwtSecurityTokenHandler().WriteToken(token) });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, message: ex.Message);
                throw;
            }
        }
    }
}
