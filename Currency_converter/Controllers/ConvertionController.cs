using Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using System.Security.Claims;
using static Common.Exceptions.ConvertionException;

namespace Currency_converter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConvertionController : ControllerBase
    {
        private readonly IConvertionService _service;
        private readonly IUserService _userService;
        public ConvertionController(IConvertionService service, IUserService userService)
        {
            _service = service;
            _userService = userService;
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetUserConvertions()
        {
            int userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "");
            try
            {
                return Ok(_service.GetUserConvertions(userId));
            }
            catch (Exception ex) {
                throw new NotAbleGetUserConvertions("We couldn´t get your convertions");
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetUserConvertionById([FromRoute] int id) 
        {
            try
            {
                var user = _userService.GetUserById(id);
                if (user != null)
                {
                    return Ok(_service.GetUserConvertions(id));
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                throw new NotAbleGetUserConvertions("We couldn´t get your convertions");
            } 
        }

        [HttpGet("user/{username}")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetUserConvertionsF([FromRoute] string username, [FromQuery] int month)
        {
            try
            {
                var user = _userService.GetUserByUsername(username);
                if (user != null)
                {
                    return Ok(_service.getUserConvertionsForMonth(username, month));
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                throw new NotAbleGetUserConvertions("We couldn´t get your convertions");
            }
        }


        [HttpPost("conv")]
        [Authorize]
        public IActionResult GetConvertion([FromBody] MakeConvertionDto conv)
        {
            int userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "");
            bool canconv = _service.canConvert(userId);
            if (canconv)
            {
                try
                {
                    return Ok(_service.MakeConvertion(userId, conv));
                }
                catch
                {
                    return BadRequest();
                }
            }
            else
            {
                return NoContent();
            }
        }
    }



    }
