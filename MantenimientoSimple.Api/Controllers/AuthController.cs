using MantenimientoSimple.Api.DTOs;
using MantenimientoSimple.Api.Services;
using Microsoft.AspNetCore.Mvc;

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
    }
}
