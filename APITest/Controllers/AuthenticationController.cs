using Utils;
using System;
using Domain.DTO;
using System.Text;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;

namespace APITest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly JWTSettings _jwtSettings;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AuthenticationController(IOptions<JWTSettings> jwtSettings, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _jwtSettings = jwtSettings.Value;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [AllowAnonymous]
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
            }

            return BadRequest("Usuário e/ou senha incorreta(s)");
        }

        [AllowAnonymous]
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
  
            var claims = new List<Claim>() {
                new Claim("Produto", "Incluir"),
                new Claim("Produto", "Editar"),
                new Claim("Produto", "Excluir"),
                new Claim("Produto", "Consultar")
            };

            var result = await _userManager.CreateAsync(user, registerUser.Password);
            result = await _userManager.AddClaimsAsync(user, claims);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            await _signInManager.SignInAsync(user, false);

            return Ok(await GenerateJwtToken(registerUser.Email));
        }

        private async Task<string> GenerateJwtToken(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            var identityClaims = new ClaimsIdentity();
            identityClaims.AddClaims(await _userManager.GetClaimsAsync(user));

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.SecretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identityClaims,
                Issuer = _jwtSettings.Sender,
                Audience = _jwtSettings.ValidURI,
                Expires = DateTime.UtcNow.AddHours(_jwtSettings.ExpirationTime),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
        }
    }
}
