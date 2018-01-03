using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HelloWin.Controllers
{
    [Produces("application/json")]
    [Route("api/Probe")]
    public class ProbeController : Controller
    {
        [HttpGet]
        public IActionResult GetAction()
        {
            return Ok();
        }
    }
}