using System.ComponentModel.DataAnnotations;

namespace MantenimientoSimple.Api.DTOs
{
    public class LoginRequest
    {
        [EmailAddress]
        public string Email { get; set; }
        
        [Required]
        public string Password { get; set; }
    }
}
