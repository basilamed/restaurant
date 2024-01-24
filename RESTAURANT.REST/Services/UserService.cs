using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RESTAURANT.REST.Data.DTOs;
using RESTAURANT.REST.Data.Models;

namespace RESTAURANT.REST.Services
{
    public class UserService : IUserSerivce
    {
        private readonly UserManager<User> userManager;
        public UserService(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<User> GetUserById(string id)
        {
            return await userManager.FindByIdAsync(id);
        }

        public async Task<List<User>> GetAllUsers()
        {
            return await userManager.Users.ToListAsync();
        }

        public async Task<User> LoginUser(UserLoginDTO model)
        {
            var user = await userManager.FindByEmailAsync(model.UserName);
            if (user == null)
            {
                throw new Exception("User not found");
            }
            var result = await userManager.CheckPasswordAsync(user, model.Password);
            if (!result)
            {
                throw new Exception("Invalid password");
            }
            return user;
        }

        public async Task<User> RegisterUser(UserRegisterDTO model)
        {
            var user = new User
            {
                Email = model.Email,
                UserName = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                Role = Enum.Parse<Role>(model.Role, true)
            };
            var result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                throw new Exception("User not created");
            }
            return user;
        }

    }
}
