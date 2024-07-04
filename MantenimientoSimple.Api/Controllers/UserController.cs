using MantenimientoSimple.Api.DTOs;
using MantenimientoSimple.Api.Models;
using MantenimientoSimple.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace MantenimientoSimple.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;
        public UsersController(
            IUserService userService
            )
        {
            _userService = userService;
        }

        [Authorize]
        [HttpGet()]
        public async Task<ActionResult<List<UserResponse>>> GetUsers()
        {
            var userList = await _userService.GetAll();
            var usersResponse = userList.Select((user) => new UserResponse() {
                 Id = user.Id,
                 UserName = user.UserName,
                 Email = user.Email,
                 Roles = user.Roles,
            });

            return Ok(usersResponse);
        }

        [Authorize(Roles = "Admin,Editor")]
        [HttpPost()]
        public async Task<ActionResult> CreateUser(CreateUserRequest request)
        {
            var roleExists = await _userService.GetRole(request.Role);
            if (roleExists == null) return StatusCode(StatusCodes.Status400BadRequest, new ErrorResponse("Rol inválido"));

            var success = await _userService.CreateUser(request);
            if (!success) return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse("Error interno al crear usuario"));

            return Ok("Usuario creado exitosamente");
        }

        [Authorize(Roles = "Admin,Editor")]
        [HttpPatch("{id}")]
        public async Task<ActionResult> UpdateUser(string Id, [FromBody] UpdateUserRequest request)
        {   
            var user = await _userService.GetById(Id);
            if (user == null) return StatusCode(StatusCodes.Status404NotFound, new ErrorResponse("Usuario no encontrado"));
            
            // edición de propiedades
            user.Email = request.Email;
            user.UserName = request.Username;

            var success = await _userService.UpdateUser(user);
            if (!success) return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse("No se pudo editar el usuario"));

            return Ok("Usuario actualizado correctamente");
        }

        [Authorize(Roles = "Admin,Editor")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(string Id)
        {
            var user = await _userService.GetById(Id);
            if (user == null) return StatusCode(StatusCodes.Status404NotFound, new ErrorResponse("Usuario no encontrado"));

            var success = await _userService.DeleteUser(user);
            if (!success) return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse("No se pudo eliminar el usuario"));

            return Ok("Usuario eliminado correctamente");
        }
    }
}
