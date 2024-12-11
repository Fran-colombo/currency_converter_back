using Common.Enum;
using Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using System.Security.Claims;

namespace Currency_converter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_userService.GetUsers());
        }

        [HttpGet("{username}")]
        public IActionResult GetUserByUsername([FromRoute] string username)
        {
            return Ok(_userService.GetUserByUsername(username));
        }
        [HttpGet("id")]
        public IActionResult GetUserById()
        {
            int userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "");
            return Ok(_userService.GetUserById(userId));
        }


        [HttpPost]
        [Authorize(Policy = "UnauthorizeIfUser")]
        public IActionResult Add([FromBody] UserForCreationDto userDto)
        {
            try
            {

                _userService.AddUser(userDto);
                return Ok("The user has been created");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPut("{username}")] 
        [Authorize(Roles ="Admin")]
        public IActionResult UpdateUserSub([FromRoute] string username, [FromBody] int subId)
        {
            try
            {
                _userService.UpdateUserSub(username, subId);
                return Ok(subId);
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException("Something went wrong, we weren´t able to update the subscription type");
            }
        }

        [HttpDelete("{username}")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteUser([FromRoute] string username) 
        {
            try
            {
                _userService.DeleteUser(username);
                return Ok();
            }
            catch (ArgumentException ex) 
            {
                throw new ArgumentException("The User wasn´t deleted");
            }
        }
    }
}
