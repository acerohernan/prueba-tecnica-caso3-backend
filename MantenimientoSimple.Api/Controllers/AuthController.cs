using MantenimientoSimple.Api.DTOs;
using MantenimientoSimple.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MantenimientoSimple.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(
            IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("Register")]
        public async Task<ActionResult> Register([FromBody] CreateUserRequest request)
        {
            // verificar si el usuario se encuentra registrado
            var userExists = await _userService.GetByEmail(request.Email);
            if (userExists != null) return StatusCode(StatusCodes.Status403Forbidden, (new ErrorResponse("Usuario ya registrado")));

            // verificar si el rol existe
            var roleExists = await _userService.GetRole(request.Role);
            if (roleExists == null) return StatusCode(StatusCodes.Status400BadRequest, new ErrorResponse("Rol inválido"));

            // crear el usuario
            var success = await _userService.CreateUser(request);
            if (!success) return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse("Error interno al crear usuario"));

            return Ok("Usuario creado exitosamente");
        }

        [HttpPost("Login")]
        public async Task<ActionResult> Login([FromBody] LoginRequest request)
        {
            // verificar si el usuario se encuentra registrado
            var user = await _userService.GetByEmail(request.Email);

            if (user == null) return StatusCode(StatusCodes.Status401Unauthorized, (new ErrorResponse("Credenciales inválidas")));
            
            // verificar si la contraseña es correcta
            var passwordIsValid = await _userService.VerifyPassword(user, request.Password);

            if (!passwordIsValid) return StatusCode(StatusCodes.Status401Unauthorized, (new ErrorResponse("Credenciales inválidas")));

            // crear jwt
            var token = await _userService.CreateToken(user);

            return Ok(new {token});
        }

        [Authorize]
        [HttpGet("Session")]
        public async Task<ActionResult> GetSession()
        {
            // Obtener el usuario del token
            var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (id == null) return Forbid();

            var user = await _userService.GetById(id);
            if (user == null) return NotFound();

            var roles = await _userService.GetRolesForUser(user);

            return Ok(new UserResponse()
            {
                 Id = user.Id,
                 Email = user.Email,
                 UserName = user.UserName,
                 Roles = roles.ToList(),
            });
        }
    }
}
