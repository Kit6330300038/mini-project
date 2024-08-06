using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MongoDB.Driver;
using my_mini_project.IServices;
using my_mini_project.ViewModel;

namespace my_mini_project.Services
{
    public class UserServices : IUserServices
    {
        private readonly IMongoDatabase _db;
        private readonly IMongoCollection<UserViewModel> _User;

        public UserServices(IMongoDatabase db)
        {
            _db = db;
            _User = _db.GetCollection<UserViewModel>("users");
        }
        public async Task<IEnumerable<UserViewModel>> GetUser()
        {
            return await _User.Find(entity => true).ToListAsync();
        }

        public async Task<UserViewModel> NewUser(UserSignUp data)
        {
            try{

                var user = new UserViewModel(){
                    username = data.username,
                    password = data.password,
                    firstname = data.firstname,
                    lastname = data.lastname,
                    email = data.email,
                    usecode = data.code
                };
                await _User.InsertOneAsync(user);
                return user;
            }catch{
                return new UserViewModel();
            }
            
        }
    }
}