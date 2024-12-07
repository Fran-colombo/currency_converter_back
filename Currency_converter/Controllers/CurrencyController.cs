using Common.Enum;
using Common.Models;
using Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using System.Security.Claims;

namespace Currency_converter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {
        private readonly ICurrencyService _service;
        public CurrencyController(ICurrencyService service)
        {
            _service = service;
        }


        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_service.GetAllCurrencies()?.OrderBy(c => c.Code));
        }

        [HttpGet("{code}")]
        public IActionResult Get([FromRoute] string code)
        {
            return Ok(_service.GetCurrencyByCode(code));
        }

        [HttpPost]
        //[Authorize(Roles = "Admin")]
        public IActionResult CreateCurrency([FromBody] CurrencyForDto currency)
        {

            _service.AddCurrency(currency);
            return Ok();
        }

        [HttpDelete("{code}")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteCurrency([FromRoute] string code)
        {
            _service.DeleteCurrency(code);
            return Ok();
        }

        [HttpPut]
        //[Authorize(Roles = "Admin")]
        public IActionResult UpdateCurrencyByCode([FromBody] CurrencyUpdateRequestDto request)
        {
            _service.UpdateCurrencyByCode(request.code, request.convertionIndex);
            return Ok();
        }

        [HttpPost("conv")]
        public IActionResult GetConvertion([FromBody] MakeConvertionDto conv)
        {
            int userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "");

            try
            {
                return Ok(_service.MakeConvertion(userId, conv));
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
