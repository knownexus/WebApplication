using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using WidgetService.Models;

namespace WidgetService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly WidgetService.Services.UserService _userService;


        public UserController(ILogger<UserController> logger, Services.UserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        [HttpGet]
        [Route("SetUser")]
        public async Task<IActionResult> SetUser([FromBody] ApplicationUser applicationUser)
        {
            var result = await _userService.SetUserDetails(applicationUser);
            if (result.successful)
            {
                return Ok(new { Success = result.successful, Message = result.message });
            }
            return Problem(detail: result.message, statusCode: StatusCodes.Status500InternalServerError); ;
        }
        [HttpGet]
        public async Task<ApplicationUser> GetByLogin([FromBody] LoginRequest login)
        {
            var result = await _userService.GetUserByLogin(login);
            return result;
        }

        [HttpPut]
        [Route("GetApiKey")]
        public async Task<IActionResult> GetApiKey([FromBody] ApplicationUser applicationUser)
        {
            var result = await _userService.GenerateApiKey(applicationUser.Id);
            if (result.successful)
            {
                return Ok(new { Success = result.successful, Message = result.message });
            }
            return Problem(detail: result.message, statusCode: StatusCodes.Status500InternalServerError);
        }


        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create([FromBody] UserDto newUser)
        {
            var result = await _userService.CreateUser(newUser.User, newUser.Password);
            if (result.successful)
            {
                return Ok(new { Success = result.successful, Message = result.message });
            }
            return Problem( detail: result.message, statusCode: StatusCodes.Status500InternalServerError);
        }
    }
}
