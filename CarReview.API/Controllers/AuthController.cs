using CarReview.API.Auth;
using CarReview.API.Auth.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CarReview.API.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("api")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<CarReviewUser> _userManager;
        private readonly IJwtTokenService _jwtTokenService;

        public AuthController(UserManager<CarReviewUser> userManager, IJwtTokenService jwTokenService)
        {
           _userManager = userManager;
           _jwtTokenService = jwTokenService;
        }

        [HttpPost]
        [Route(template: "register")]
        public async Task<IActionResult> Register(RegisterUserDTO registerUserDTO)
        {
            var user = await _userManager.FindByNameAsync(registerUserDTO.UserName);

            if (user != null)
            {
                return BadRequest("Request invalid!");
            }

            var newUser = new CarReviewUser
            {
                Email = registerUserDTO.Email,
                UserName = registerUserDTO.UserName
            };

            var createUserResult = await _userManager.CreateAsync(newUser, registerUserDTO.Password);

            if (!createUserResult.Succeeded)
            {
                return BadRequest("Could not create a user!");
            }

            await _userManager.AddToRoleAsync(newUser, role: CarReviewRoles.ReviewUser);

            return CreatedAtAction(nameof(Register), value: new UserDTO(newUser.Id, newUser.UserName, newUser.Email));
        }

        [HttpPost]
        [Route(template: "login")]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            var user = await _userManager.FindByNameAsync(loginDTO.UserName);

            if (user == null)
            {
                return BadRequest("User name or password is invalid!");
            }

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, loginDTO.Password);

            if (!isPasswordValid)
            {
                return BadRequest("User name or password is invalid!");
            }

            var roles = await _userManager.GetRolesAsync(user);

            var accessToken = _jwtTokenService.CreateAccessToken(user.UserName, user.Id, roles);

            return Ok(new SuccessfulLoginDTO(accessToken));
        }
    }
}
