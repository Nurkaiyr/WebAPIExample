using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Modules;
using Services;

namespace WebAPIExample.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly UserServices _userServices;

        public AuthController(UserServices userServices)
        {
            _userServices = userServices;
        }

        [HttpPost]
        public async Task<IActionResult> Authenticate([FromBody]User user)
        {
            var token = await _userServices.Authenticate(user.Login, user.Password);
            if(string.IsNullOrEmpty(token))
                return BadRequest();

            return Ok(new {token });
        }
    }
}