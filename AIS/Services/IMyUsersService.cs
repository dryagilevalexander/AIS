using AIS.ViewModels;
using Infrastructure;
using Infrastructure.Models;

namespace AIS.Services
{
    public interface IMyUsersService
    {
        Task<User> GetCurrentUser(string userName);
        Task<User> GetUserById(string id);
        Task<List<User>> GetUsers();
    }
}
