using Microsoft.AspNetCore.Mvc;
using Core;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace AIS.Services
{
    public class MyUsersService: IMyUsersService
    {
        private CoreContext db;

        public MyUsersService(CoreContext coreContext)
        {
            db = coreContext;
        }
        public async Task<User> GetCurrentUser(string userName)
        {
             var currentUser = db.Users.FirstOrDefault(p => p.UserName == userName);
             return currentUser;
        }

        public async Task<User> GetUserById(string id)
        {
            User user = db.Users.FirstOrDefault(p => p.Id == id);
            return user;
        }

        public async Task <List<User>> GetUsers()
        {
            List<User> users = db.Users.ToList();
            return users;
        }
    }
}
