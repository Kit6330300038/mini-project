using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace my_mini_project.IServices
{
    public interface IAccountServices
    {
        Task<bool> Login(string username , string password);
        Task<Claim[]> getCaim(string username);
        Task<string> GenerateToken(string username);
    }
}