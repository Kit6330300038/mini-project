using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
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
        public UserController(IUserServices userServices){
            _userServices = userServices;
        }
        [HttpGet("[action]")]
        public async Task<IEnumerable<UserViewModel>> getAllUser()
        {
            
            return await _userServices.GetUser();
        }
        [HttpPost("[action]")]
        public async Task<UserViewModel> Register(UserSignUp data)
        {
            
            return await _userServices.NewUser(data);
        }


    }
}