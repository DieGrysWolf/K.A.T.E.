using Domain.Interfaces;
using Domain.Models.DTOs;
using Domain.Models.DTOs.Auth;
using Domain.Models.Entity;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly MariaDBContext _context;
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(MariaDBContext context, ILogger<UserRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<UserModel> GetUserById(Guid userId)
        {
            try
            {
                return await _context.Users.FindAsync(userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception($"An error occurred while retrieving the user with ID {userId}: {ex.Message}", ex);
            }
        }

        public async Task<UserModel> GetUserByEmail(string email)
        {
            try
            {
                return await _context.Users
                    .Include(u => u.Roles)
                    .FirstOrDefaultAsync(u => u.EmailAddress == email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception($"An error occurred while retrieving the user with email {email}: {ex.Message}", ex);
            }
        }

        public async Task<List<UserDTO>> GetAllUsers()
        {
            try
            {
                var users = await _context.Users.ToListAsync();

                var usersDTO = users.Select(u => new UserDTO
                {
                    Id = u.Id,
                    FullName = u.FullName,
                    AccessLevel = u.AccessLevel,
                    EmailAddress = u.EmailAddress,
                    HasReportAccess = u.HasReportAccess
                }).ToList();

                return usersDTO;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception($"An error occurred while retrieving all users: {ex.Message}", ex);
            }
        }

        public async Task<bool> AddUser(UserModel user)
        {
            try
            {
                await _context.Users.AddAsync(user);
                var entries = await _context.SaveChangesAsync();

                return entries > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception($"An error occurred while adding the user: {ex.Message}", ex);
            }
        }
    }
}
