using BCrypt.Net;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskManagementAPI.Configurations;
using TaskManagementAPI.DTOs;
using TaskManagementAPI.Models;
using TaskManagementAPI.Repositories;
using Task = System.Threading.Tasks.Task;

namespace TaskManagementAPI.Services
{
    public class UserService : IUserService
    {
        private readonly IUserDbRepository _userDbRepository;
        private readonly IConfiguration _configuration;
        private readonly ILogger<UserService> _logger;

        public UserService(IUserDbRepository userDbRepository, IConfiguration configuration, ILogger<UserService> logger)
        {
            _userDbRepository = userDbRepository;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task RegisterUserAsync(UserRegisterDTO userDto)
        {
            try
            {
                var user = new User
                {
                    Username = userDto.Username,
                    Email = userDto.Email,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password)
                };

                await _userDbRepository.AddAsync(user);
                _logger.LogInformation("User {Username} successfully registered.", user.Username);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while registering user {Username}.", userDto.Username);
                throw;
            }
        }

        public async Task<string> AuthenticateUserAsync(UserLoginDTO userDto)
        {
            try
            {
                var user = await _userDbRepository.GetUserByUsernameOrEmailAsync(userDto.UsernameOrEmail);
                if (user == null || !BCrypt.Net.BCrypt.Verify(userDto.Password, user.PasswordHash))
                {
                    _logger.LogWarning("Failed login attempt for username or email {UsernameOrEmail}.", userDto.UsernameOrEmail);
                    throw new UnauthorizedAccessException("Invalid username or password.");
                }

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration["JwtConfig:Secret"]);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim(ClaimTypes.Name, user.Username)
                    }),
                    Expires = DateTime.UtcNow.AddHours(2),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                _logger.LogInformation("User {Username} authenticated and token issued.", user.Username);

                return tokenHandler.WriteToken(token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during authentication for user {UsernameOrEmail}.", userDto.UsernameOrEmail);
                throw;
            }
        }

        public async Task<User> GetUserByIdAsync(Guid id)
        {
            return await _userDbRepository.GetByIdAsync(id);
        }
    }
}
