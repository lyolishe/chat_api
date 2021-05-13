using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using chat_api.Models;
using chat_api.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using MongoDB.Driver;

namespace chat_api.Services
{
    public class UserService
    {
        private readonly IMongoCollection<User> _users;

        public UserService(DatabaseSettings databaseSettings)
        {
            var client = new MongoClient();
            var database = client.GetDatabase(databaseSettings.DataBase);

            _users = database.GetCollection<User>("User");
        }

        public List<User> Get() => _users.Find(user => true).ToList();

        public User Get(string id) => _users.Find(user => user.Id == id).FirstOrDefault();

        public User Create(User user)
        {
            user.Id = Guid.NewGuid().ToString();
            user.CreationDate = DateTime.Now;
            
            _users.InsertOne(user);
            return user;
        }

        public User Update(string id, User newUser)
        {
            _users.ReplaceOne(user => user.Id == id, newUser);

            return newUser;
        }

        public void Remove(string id)
        {
            _users.DeleteOne(user => user.Id == id);
        }
        
    }
}