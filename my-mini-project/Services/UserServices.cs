using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
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

        private readonly List<int> gainperLot = new List<int>{5,3,2,1,0};

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
                if (!String.IsNullOrEmpty(user.usecode))
                {
                    user.descending = await AddCommisionMoney(user);
                }
                await _User.InsertOneAsync(user);

                return user;
            }
            catch(Exception ex)
            {
                return new UserViewModel();
            }

        }


        public async Task<string> getCode(string username)
        {
            var filter = Builders<UserViewModel>.Filter.Eq(b => b.username, username);
            var data = await _User.Find(filter).FirstOrDefaultAsync();
            if (string.IsNullOrEmpty(data.selfcode) && data.descending < 3)
            {
                string code;
                do
                {
                    code = await GenerateCode();
                } while (await IsCodeInDatabaseAsync(code));

                var update = Builders<UserViewModel>.Update.Set(b => b.selfcode, code);
                await _User.UpdateOneAsync(filter, update);
                return code;
            }if(data.descending == 3)
            {
                return "";
            }
            return data.selfcode;
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
            return data.gain * gainperLot[data.descending+1];
        }

        public int Lot(UserViewModel model)
        {
            return max(2, min(30, model.firstname.Length + model.lastname.Length + model.username.Length));
        }
        public int max(int num1, int num2)
        {
            return num1 > num2 ? num1 : num2;
        }
        public int min(int num1, int num2)
        {
            return num1 < num2 ? num1 : num2;
        }

        public static async Task<string> GenerateCode()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                var key = new byte[32];
                rng.GetBytes(key);
                return Convert.ToBase64String(key);
            }
        }
        private async Task<bool> IsCodeInDatabaseAsync(string code)
        {
            var filter = Builders<UserViewModel>.Filter.Eq(d => d.selfcode, code);
            var count = await _User.CountDocumentsAsync(filter);
            return count > 0;
        }
        private async Task<int> AddCommisionMoney(UserViewModel user)
        {
            var filter = Builders<UserViewModel>.Filter.Eq(b => b.selfcode, user.usecode);
            var data = await _User.Find(filter).FirstOrDefaultAsync();
            var update = Builders<UserViewModel>.Update.Set(b => b.gain, (data.gain + Lot(user)));
            await _User.UpdateOneAsync(filter, update);
            return data.descending+1;
        }
    }
}