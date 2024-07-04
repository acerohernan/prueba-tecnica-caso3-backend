namespace MantenimientoSimple.Api.Models
{
    public class UserWithRoles: User
    {
        public List<string> Roles { get; set; } = new List<string>();
    }
}
