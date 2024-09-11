using TaskManagementAPI.Data;
using TaskManagementAPI.Models;
using Microsoft.EntityFrameworkCore;
using TaskManagementAPI.Data.TaskManagementAPI.Data;

namespace TaskManagementAPI.Repositories
{
    public class UserDbRepository : BaseRepository<User>, IUserDbRepository
    {
        public UserDbRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<User?> GetUserByUsernameOrEmailAsync(string usernameOrEmail)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Username == usernameOrEmail || u.Email == usernameOrEmail);
        }
    }
}
