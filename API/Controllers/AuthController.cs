using API.Services.AccessPoint;
using API.Services.Auth;
using Domain.Models.DTOs.Auth;
using Domain.Models.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IAuthService _authService;

        public AuthController(IConfiguration configuration, IAuthService authService)
        {
            _configuration = configuration;
            _authService = authService;
        }

        [HttpGet("DEMO-ONLY-get-all-users")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _authService.GetAllUsers();

                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("sign-up")]
        public async Task<ActionResult<UserModel>> Signup([FromBody] SignupModel signupDTO)
        {
            try
            {
                _authService.CreatePasswordHash(signupDTO.Password, out byte[] passwordHash, out byte[] passwordSalt);

                var user = new UserModel
                {
                    Id = Guid.NewGuid(),
                    FullName = signupDTO.FullName,
                    EmailAddress = signupDTO.EmailAddress,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                    AccessLevel = signupDTO.AccessLevel,
                    HasReportAccess = signupDTO.isAdmin,
                    Roles = new List<Role> { new Role { Name = signupDTO.isAdmin ? "Admin" : "User" } }
                };

                var success = await _authService.CreateUser(user);

                if (success)
                    return Created("Success", user.EmailAddress);
                else
                    return BadRequest();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("sign-in")]
        public async Task<ActionResult<string>> Signin([FromBody] SigninModel signinDTO)
        {
            try
            {
                var user = await _authService.GetUserByEmail(signinDTO.EmailAddress);

                if (user == null)
                    return Unauthorized("Invalid username or password");

                if (!_authService.VerifyPasswordHash(signinDTO.Password, user.PasswordHash, user.PasswordSalt))
                    return Unauthorized("Invalid username or password");

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.EmailAddress),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                };

                foreach (var role in user.Roles)
                    claims.Add(new Claim(ClaimTypes.Role, role.Name));

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.Now.AddDays(1),
                    SigningCredentials = creds,
                    Audience = _configuration["Jwt:Issuer"],
                    Issuer = _configuration["Jwt:Issuer"]
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);

                return Ok(new
                {
                    token = tokenHandler.WriteToken(token)
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }            
        }
    }
}
