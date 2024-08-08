using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using my_mini_project.ViewModel;

namespace my_mini_project.IServices
{
    public interface IUserServices
    {
        Task<IEnumerable<UserViewModel>> GetUser();
        Task<UserViewModel> NewUser(UserSignUp data);
        Task<string> getCode(string? username);
        Task<int> getUserLot(string? username);
        Task<int> getCommisionMoney(string? username);
    }
}