using Common.Enum;
using Common.Exceptions;
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
        [Authorize]
        public IActionResult Get()
        {
            return Ok(_service.GetAllCurrencies()?.OrderBy(c => c.Code));
        }

        [HttpGet("{code}")]
        [Authorize]
        public IActionResult Get([FromRoute] string code)
        {
            return Ok(_service.GetCurrencyByCode(code));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult CreateCurrency([FromBody] CurrencyForDto currency)
        {
            try
            {
                _service.AddCurrency(currency);
                return Ok();
            }
            catch (CurrencyAlreadyExistException ex)
            {
                return Conflict(new { message = ex.Message });  // Código 409 para conflicto
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        [HttpDelete("{code}")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteCurrency([FromRoute] string code)
        {
            _service.DeleteCurrency(code);
            return Ok();
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public IActionResult UpdateCurrencyByCode([FromBody] CurrencyUpdateRequestDto request)
        {
            _service.UpdateCurrencyByCode(request.code, request.convertionIndex);
            return Ok();
        }

    }
}
