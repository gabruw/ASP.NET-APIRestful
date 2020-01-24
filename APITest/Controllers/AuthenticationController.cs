using System.Linq;
using System.Threading.Tasks;
using Domain.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace APITest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AuthenticationController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
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

            return Ok("Registrado com sucesso!");
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
                return Ok("Logado com sucesso!");
            }

            return BadRequest("Usuário e/ou senha incorreta(s)");
        }
    }
}
