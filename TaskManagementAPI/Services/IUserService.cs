using TaskManagementAPI.DTOs;
using TaskManagementAPI.Models;
using Task = System.Threading.Tasks.Task;

namespace TaskManagementAPI.Services
{
    public interface IUserService
    {
        Task RegisterUserAsync(UserRegisterDTO userDto);
        Task<string> AuthenticateUserAsync(UserLoginDTO userDto);
        Task<User> GetUserByIdAsync(Guid id);
    }
}
