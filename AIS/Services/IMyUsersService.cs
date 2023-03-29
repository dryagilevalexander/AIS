using AIS.ViewModels;
using Core;

namespace AIS.Services
{
    public interface IMyUsersService
    {
        Task<User> GetCurrentUser(string userName);
        Task<User> GetUserById(string id);
        Task<List<User>> GetUsers();
    }
}
