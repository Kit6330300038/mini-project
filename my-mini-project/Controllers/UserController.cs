using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using my_mini_project.IServices;
using my_mini_project.ViewModel;

namespace my_mini_project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserServices _userServices;
        private readonly IAccountServices _accountServices;
        public UserController(IUserServices userServices, IAccountServices accountServices)
        {
            _accountServices = accountServices;
            _userServices = userServices;
        }
        // [HttpGet("[action]")]
        // public async Task<IEnumerable<UserViewModel>> getAllUser()
        // {

        //     return await _userServices.GetUser();
        // }
        [HttpPost("[action]")]
        public async Task<UserViewModel> Register(UserSignUp data)
        {
            return await _userServices.NewUser(data);
        }
        [Authorize]
        [HttpGet("[action]")]
        public async Task<int> getUserLot()
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value.ToString();
            return await _userServices.getUserLot(username);
        }
        [Authorize]
        [HttpGet("[action]")]
        public async Task<int> moneyGain()
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value.ToString();
            return await _userServices.getCommisionMoney(username);
        }
        [Authorize]
        [HttpGet("[action]")]
        public async Task<string> getCode()
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value.ToString();
            return await _userServices.getCode(username);
        }


    }
}