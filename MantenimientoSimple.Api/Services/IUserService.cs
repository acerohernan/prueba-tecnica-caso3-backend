using MantenimientoSimple.Api.DTOs;
using MantenimientoSimple.Api.Models;
using Microsoft.AspNetCore.Identity;

namespace MantenimientoSimple.Api.Services
{
    public interface IUserService
    {
        Task<User> GetByEmail(string email);

        Task<User> GetById(string id);
        Task<bool> CreateUser(CreateUserRequest request);
        Task<bool> UpdateUser(User user);

        Task<bool> DeleteUser(User user);
        Task<List<string>> GetRolesForUser(User user);

        Task<bool> VerifyPassword(User user, string password);

        Task<string> CreateToken(User user);

        Task<List<UserWithRoles>> GetAll();

        Task<IdentityRole> GetRole(string roleName);
    }
}
