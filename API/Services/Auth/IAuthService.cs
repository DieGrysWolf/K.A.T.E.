using Domain.Models.DTOs.Auth;
using Domain.Models.Entity;

namespace API.Services.Auth
{
    public interface IAuthService
    {
        Task<bool> CreateUser(UserModel user);
        Task<UserModel> GetUserByEmail(string email);
        Task<List<UserDTO>> GetAllUsers();
        void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
        bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt);
    }
}