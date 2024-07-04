using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MantenimientoSimple.Api.DTOs
{
    public class UpdateUserRequest
    {
        [Required, MinLength(6), MaxLength(20)]
        public string Username { get; set; }
        [EmailAddress]
        public string Email { get; set; }
    }
}
