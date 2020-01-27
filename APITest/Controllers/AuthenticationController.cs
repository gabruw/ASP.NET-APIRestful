using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Utils;

namespace APITest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly JWTSettings _jwtSettings;

        public AuthenticationController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IOptions<JWTSettings> jwtSettings)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtSettings = jwtSettings.Value;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterUserDTO registerUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values.SelectMany(e => e.Errors));
            }

            var user = new IdentityUser
            {
                UserName = registerUser.Username,
                Email = registerUser.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, registerUser.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            await _signInManager.SignInAsync(user, false);

            return Ok(await GenerateJwtToken(registerUser.Email));
            //return Ok("Registrado com sucesso!");
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginUserDTO loginUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values.SelectMany(e => e.Errors));
            }

            var result = new Microsoft.AspNetCore.Identity.SignInResult();
            if (!string.IsNullOrEmpty(loginUser.Username))
            {
                result = await _signInManager.PasswordSignInAsync(loginUser.Username, loginUser.Password, false, true);
            }
            else if (string.IsNullOrEmpty(loginUser.Username) && !string.IsNullOrEmpty(loginUser.Email))
            {
                var user = await _userManager.FindByEmailAsync(loginUser.Email);
                result = await _signInManager.PasswordSignInAsync(user.UserName, loginUser.Password, false, true);
            }

            if (result.Succeeded)
            {
                return Ok(await GenerateJwtToken(loginUser.Email));
                //return Ok("Logado com sucesso!");
            }

            return BadRequest("Usuário e/ou senha incorreta(s)");
        }

        private async Task<string> GenerateJwtToken(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.SecretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _jwtSettings.Sender,
                Audience = _jwtSettings.ValidURI,
                Expires = DateTime.UtcNow.AddHours(_jwtSettings.ExpirationTime),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
        }
    }
}
