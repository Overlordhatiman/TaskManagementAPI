using TaskManagementAPI.Models;

namespace TaskManagementAPI.Repositories
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User?> GetUserByUsernameOrEmailAsync(string usernameOrEmail);
    }
}
