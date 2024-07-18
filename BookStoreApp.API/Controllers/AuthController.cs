using AutoMapper;
using BookStoreApp.API.Data;
using BookStoreApp.API.Models.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthController(ILogger<AuthController> logger, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(UserDto userDto)
        {
            _logger.LogInformation($"Registration attempt for {userDto.Email}");

            try
            {
                var user = _mapper.Map<ApplicationUser>(userDto);
                user.UserName = userDto.Email;
                var result = await _userManager.CreateAsync(user, userDto.Password);

                if (!result.Succeeded)
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError(item.Code, item.Description);
                    }
                    return BadRequest(ModelState);
                }
                
                await _userManager.AddToRoleAsync(user, "User");
                return Accepted();

            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Something went wrong with registration {nameof(Register)}");
                return Problem($"Something went wrong with registration {nameof(Register)}", statusCode: 500);
            }
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginUserDto userDto)
        {
            _logger.LogInformation($"Login attempt was made for {userDto.Email}");
            try
            {
                var user = await _userManager.FindByEmailAsync(userDto.Email);
                var passwordValid = await _userManager.CheckPasswordAsync(user, userDto.Password);

                if (user == null || !passwordValid)
                {
                    return NotFound();
                }
                return Accepted();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Something went wrong with login {nameof(Login)}");
                return Problem($"Something went wrong with login {nameof(Login)}", statusCode: 500);
            }
        }
    }
}
