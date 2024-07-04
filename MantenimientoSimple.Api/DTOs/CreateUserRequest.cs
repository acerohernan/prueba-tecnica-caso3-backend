using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MantenimientoSimple.Api.DTOs
{
    public class CreateUserRequest
    {
        [Required, MinLength(6), MaxLength(20)]
        public string Username { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        // TODO: validar que el password lleve un alphanumeric, un digito y un uppercase
        [Required]
        public string Password { get; set; }
        [Required, Compare("Password")]
        public string ConfirmPassword { get; set; }

        public string Role {  get; set; }
    }
}
