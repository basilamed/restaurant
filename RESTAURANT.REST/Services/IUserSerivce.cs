using RESTAURANT.REST.Data.DTOs;
using RESTAURANT.REST.Data.Models;

namespace RESTAURANT.REST.Services
{
    public interface IUserSerivce
    {
        Task<User> RegisterUser(UserRegisterDTO model);
        Task<User> LoginUser(UserLoginDTO model);
        Task<User> GetUserById(string id);
        Task <List<User>> GetAllUsers();
    }
}
