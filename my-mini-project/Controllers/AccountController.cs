using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using my_mini_project.IServices;

namespace my_mini_project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountServices _AccountServices;
        public AccountController(IAccountServices accountServices)
        {
            _AccountServices = accountServices;
        }

        [HttpGet("[action]")]
        public async Task<string> getLoginToken(string username, string password)
        {
            if (await _AccountServices.Login(username, password))
            {
                return await _AccountServices.GenerateToken(username);
            }
            return "";
        }


        // [HttpPost("login")]
        // public async Task<IActionResult> Login(string username, string password)
        // {
        //     if (await _AccountServices.Login(username, password))
        //     {
        //         var claims = await _AccountServices.getCaim(username);
        //         var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        //         await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
        //         return Ok(new { Message = "Login successful" });
        //     }

        //     return Unauthorized();
        // }

        // [HttpPost("logout")]
        // public async Task<IActionResult> Logout()
        // {
        //     await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        //     return Ok(new { Message = "Logout successful" });
        // }

    }
}