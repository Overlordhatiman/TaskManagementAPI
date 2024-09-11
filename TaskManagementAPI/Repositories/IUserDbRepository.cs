using TaskManagementAPI.Models;

namespace TaskManagementAPI.Repositories
{
    public interface IUserDbRepository : IBaseRepository<User>
    {
        public Task<User?> GetUserByUsernameOrEmailAsync(string usernameOrEmail);
    }
}
