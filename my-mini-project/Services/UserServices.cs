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
            try
            {
                var user = new UserViewModel()
                {
                    username = data.username,
                    password = data.password,
                    firstname = data.firstname,
                    lastname = data.lastname,
                    email = data.email,
                    usecode = data.code
                };
                await _User.InsertOneAsync(user);
                if (!String.IsNullOrEmpty(user.usecode))
                { }
                return user;
            }
            catch
            {
                return new UserViewModel();
            }

        }
        public async Task<int> getUserLot(string username)
        {
            var filter = Builders<UserViewModel>.Filter.Eq(b => b.username, username);
            var data = await _User.Find(filter).FirstOrDefaultAsync();
            return Lot(data);
        }
        public async Task<int> getCommisionMoney(string username)
        {
            var filter = Builders<UserViewModel>.Filter.Eq(b => b.username, username);
            var data = await _User.Find(filter).FirstOrDefaultAsync();
            return data.gain;
        }
        public int Lot(UserViewModel model)
        {
            return max(2, min(30, model.firstname.Length + model.lastname.Length + model.username.Length));
        }
        public int max(int num1, int num2)
        {
            return (num1 > num2 ? num1 : num2);
        }
        public int min(int num1, int num2)
        {
            return (num1 < num2 ? num1 : num2);
        }
    }
}