using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace LoggingAndErrors.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class SecureController : ControllerBase
    {
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Get()
        {
            return Ok("Vous êtes authentifié");
        }
        [HttpGet("Anonyme")]
        public IActionResult GatAnonyme()
        {
            return Ok("Accès anonyme");
        }
    }
}
