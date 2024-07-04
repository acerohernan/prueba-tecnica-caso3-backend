using MantenimientoSimple.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace MantenimientoSimple.Api.Data
{
    public class AuthDbContext : IdentityDbContext<User>
    {
        public AuthDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            ConfigurarNombreDeTablas(builder);
            GenerarRolesPorDefecto(builder);
        }
        
        /// <summary>
        /// Reemplaza los nombres de tabla por defecto de 'AspNetCode.Identity'
        /// </summary>
        /// <param name="builder">Constructor de los modelos</param>
        private void ConfigurarNombreDeTablas(ModelBuilder builder) {
            builder.Entity<User>(entity =>
            {
                entity.ToTable(name: "Users");
            });

            builder.Entity<IdentityRole>(entity =>
            {
                entity.ToTable(name: "Roles");
            });

            builder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.ToTable("UserRoles");
            });

            builder.Entity<IdentityUserClaim<string>>(entity =>
            {
                entity.ToTable("UserClaims");
            });

            builder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.ToTable("UserLogins");
            });

            builder.Entity<IdentityRoleClaim<string>>(entity =>
            {
                entity.ToTable("RoleClaims");
            });

            builder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.ToTable("UserTokens");
            });
        }

        /// <summary>
        /// Genera la información de roles por defecto
        /// </summary>
        /// <param name="builder">Constructor de los modelos</param>
        private void GenerarRolesPorDefecto(ModelBuilder builder)
        {
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole()
                {
                    Name = "Admin",
                    ConcurrencyStamp = "1",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole() {
                    Name = "Editor",
                    ConcurrencyStamp = "2",
                    NormalizedName = "EDITOR"
                },
                new IdentityRole()
                {
                    Name = "Viewer",
                    ConcurrencyStamp = "3",
                    NormalizedName = "VIEWER"
                }
            );
        }
    }
}
