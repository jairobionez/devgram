using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Devgram.Api.Controllers
{
    [Route("teste")]
    public class TesteController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> teste(){
            return Ok();
        }
    }
}