using Domain.Interfaces;
using Domain.Models.DTOs.Auth;
using Domain.Models.Entity;

namespace API.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;

        public AuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> CreateUser(UserModel user)
        {
            try
            {
                var success = await _userRepository.AddUser(user);

                return success;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<UserDTO>> GetAllUsers()
        {
            try
            {
                return await _userRepository.GetAllUsers();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<UserModel> GetUserByEmail(string email)
        {
            try
            {
                return await _userRepository.GetUserByEmail(email);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            try
            {
                using (var hmac = new System.Security.Cryptography.HMACSHA512())
                {
                    passwordSalt = hmac.Key;
                    passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }
            return true;
        }
    }
}
