using MantenimientoSimple.Api.Data;
using MantenimientoSimple.Api.DTOs;
using MantenimientoSimple.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MantenimientoSimple.Api.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AuthDbContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly ILogger<UserService> _logger;

        public UserService(
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            AuthDbContext dbContext,
            IConfiguration configuration,
            ILogger<UserService> logger
            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _dbContext = dbContext;
            _configuration = configuration;
            _logger = logger;
        }

        /// <summary>
        /// Obtener un usuario con su email
        /// </summary>
        /// <param name="email">Email de usuario</param>
        /// <returns>Usuario</returns>
        public Task<User> GetByEmail(string email)
        {
            return _userManager.FindByEmailAsync(email);
        }

        /// <summary>
        /// Obtener un usuario con su id
        /// </summary>
        /// <param name="id">Id de usuario</param>
        /// <returns>Usuario</returns>
        public Task<User> GetById(string id)
        {
            return _userManager.FindByIdAsync(id);
        }

        /// <summary>
        /// Crear usuario desde una request
        /// </summary>
        /// <param name="request">Request de crear usuario</param>
        /// <returns></returns>
        public async Task<bool> CreateUser(CreateUserRequest request)
        {
            // crear nuevo usuario y guardar en base de datos
            var user = new User()
            {
                Email = request.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = request.Username,
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                _logger.LogError($"Error al crear nuevo usuario: {result.ToString()}");
                return false;
            }

            // asignar un rol
            result = await _userManager.AddToRoleAsync(user, request.Role);

            if (!result.Succeeded)
            {
                _logger.LogError($"Error al asignar un rol al nuevo usuario: {result.ToString()}");
                
                // si existe un error al asignar el rol, eliminamos el usuario
                await _userManager.DeleteAsync(user);
                _logger.LogInformation($"Usuario eliminado");
                
                return false;
            }

            return true;
        }

        /// <summary>
        /// Modifica un usuario
        /// </summary>
        /// <param name="user">Usuario</param>
        /// <returns>Si el resultado fue correcto o incorrecto</returns>
        public async Task<bool> UpdateUser(User user)
        {
            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }

        /// <summary>
        /// Elimina un usuario
        /// </summary>
        /// <param name="user">Usuario</param>
        /// <returns>Si el resultado fue correcto o incorrecto</returns>
        public async Task<bool> DeleteUser(User user)
        {
            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded;
        }

        /// <summary>
        /// Verificar si la contraseña es correcta
        /// </summary>
        /// <param name="user">Usuario</param>
        /// <param name="password">Contraseña a verificar</param>
        /// <returns></returns>
        public Task<bool> VerifyPassword(User user, string password)
        {
           return _userManager.CheckPasswordAsync(user, password);
        }

        /// <summary>
        /// Crear JWT token para usuario
        /// </summary>
        /// <param name="user">Usuario</param>
        /// <returns>Token en cadena de texto</returns>
        public async Task<string> CreateToken(User user)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var roles = await _userManager.GetRolesAsync(user);

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("JWT:Secret").Value));

            var jwt = new JwtSecurityToken(
                issuer: _configuration.GetSection("JWT:ValidIssuer").Value,
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
                );

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        /// <summary>
        /// Obtener todos los usuarios con roles
        /// </summary>
        /// <returns>Lista de todos los usuarios con roles</returns>
        public Task<List<UserWithRoles>> GetAll()
        {
            var query = from user in _dbContext.Users
                        join userRole in _dbContext.UserRoles on user.Id equals userRole.UserId
                        join role in _dbContext.Roles on userRole.RoleId equals role.Id
                        select new UserWithRoles
                        {
                            Id = user.Id,
                            UserName = user.UserName,
                            Email = user.Email,
                            Roles = _dbContext.UserRoles
                            .Where(ur => ur.UserId == user.Id)
                                           .Join(_dbContext.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => r.Name)
                                           .ToList()
                        };

            return query.ToListAsync();

        }

        /// <summary>
        /// Obtener role con el nombre
        /// </summary>
        /// <param name="roleName">Nombre del rol</param>
        /// <returns>Rol</returns>
        public Task<IdentityRole> GetRole(string roleName)
        {
            return _roleManager.FindByNameAsync(roleName);
        }
    }
}
