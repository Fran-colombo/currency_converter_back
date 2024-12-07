using Common.Enum;
using Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Implementations;
using Services.Interfaces;

namespace Currency_converter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionController : ControllerBase
    {
        private readonly ISubscriptionService _service;
        public SubscriptionController(ISubscriptionService service)
        {
            _service = service;
        }

        [HttpPost]
        [Authorize]
        public IActionResult CreateSubscription(Subscription sub)
        {

            _service.CreateSubscription(sub);
            var subt = sub.SubscriptionType;
            return Ok(subt);
        }

        [HttpGet]
        public IActionResult GetSubscriptions() { 

            var allsubs = _service.GetAllSubscriptions();
            return Ok(allsubs);
        }

        [HttpPut("{username}")]
        [Authorize]
        public IActionResult ChangeSubscription([FromRoute] string username, [FromBody] int subId)
        {
            _service.ChangeSubscription(username, subId);
            return Ok(subId);
        }

    }
}
