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
        //[Authorize(Roles = "Admin")]
        [Authorize]
        public IActionResult Get()
        {
            return Ok(_userService.GetUsers());
        }

        [HttpGet("{username}")]
        [Authorize]
        public IActionResult GetUserByUsername([FromRoute] string username)
        {
            return Ok(_userService.GetUserByUsername(username));
        }
        [HttpGet("id")]
        [Authorize]
        public IActionResult GetUserById()
        {
            int userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "");
            return Ok(_userService.GetUserById(userId));
        }


        //[HttpPost]
        //[Authorize(Policy = "UnauthorizeIfUser")]
        //public IActionResult Add([FromBody] UserForCreationDto userDto)
        //{
        //    var user = _userService.GetUserByUsername(userDto.Username);
        //    var email = _userService.GetUserByEmail(userDto.Email);

        //    if (user == null && email == null)
        //    {
        //    try
        //    {

        //        _userService.AddUser(userDto);
        //        return Ok("The user has been created");
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //    }
        //    else if (user != null)
        //    {
        //        return BadRequest(new { message = "Username already exists", type = "username" });
        //    }
        //    // Si el email ya existe, retornamos un error específico para el email
        //    else 
        //    {
        //        return Conflict(new { message = "Email already exists", type = "email" });
        //    }
        //}

        [HttpPost]
        [Authorize(Policy = "UnauthorizeIfUser")]
        public IActionResult Add([FromBody] UserForCreationDto userDto)
        {
            var user = _userService.GetUserByUsername(userDto.Username);
            var email = _userService.GetUserByEmail(userDto.Email);

            if (user == null && email == null)
            {
                try
                {
                    _userService.AddUser(userDto);
                    return Ok(new { success = true, message = "User created successfully" });
                }
                catch (Exception ex)
                {
                    return StatusCode(500, new { success = false, message = "An error occurred while creating the user: " + ex.Message });
                }
            }
            else if (user != null)
            {
                return BadRequest(new { success = false, message = "The username already exists", type = "username" });
            }
            else if (email != null)
            {
                return BadRequest(new { success = false, message = "The email already exists", type = "email" });
            }
            return BadRequest(new { success = false, message = "An unknown error occurred" });
        }


        [HttpPut("{username}")]
        [Authorize(Roles ="Admin")]
        public IActionResult UpdateUserSubAsAdmin([FromRoute] string username, [FromBody] int subId)
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
        [HttpPut]
        [Authorize]
        public IActionResult UpdateUserSub([FromBody] int subId)
        {
            int userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "");
            var user = _userService.GetUserById(userId);
            try
            {
                _userService.UpdateUserSub(user.Username, subId);
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
