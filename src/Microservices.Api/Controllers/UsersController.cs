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
    public class UsersController : Controller
    {

        private readonly IBusClient _busClient;
        public UsersController(IBusClient bus)
        {
            _busClient = bus;
        }

        [HttpGet]
        public IActionResult Get() => Content("helo activities");

        [HttpPost("register")]
        public async Task Register([FromBody] CreateUser command)
        {
            await _busClient.PublishAsync(command);
        }
    }
}