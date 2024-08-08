using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace my_mini_project.IServices
{
    public interface IAccountServices
    {
        Task<string> Login(string username , string password);
    }
}