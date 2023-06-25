using Domain.Models.DTOs.Auth;
using Domain.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<UserModel> GetUserById(Guid userId);
        Task<UserModel> GetUserByEmail(string email);
        Task<List<UserDTO>> GetAllUsers();
        Task<bool> AddUser(UserModel user);
    }
}
