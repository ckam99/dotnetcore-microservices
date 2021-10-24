using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microservices.Common.Commands;
using Microsoft.AspNetCore.Mvc;
using RawRabbit;

namespace Microservices.Api.Controllers
{

    [Route("[controller]")]
    public class ActivitiesController : Controller
    {

        private readonly IBusClient _busClient;
        public ActivitiesController(IBusClient bus)
        {
            _busClient = bus;
        }

        [HttpGet]
        public IActionResult Get() => Content("helo activities");

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateActivity command)
        {
            command.Id = Guid.NewGuid();
            command.CreatedAt = DateTime.Now;
            await _busClient.PublishAsync(command);
            return Accepted($"activities/{command.Id}");
        }
    }
}