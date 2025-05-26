using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Blogify.API.Data
{
    public class AuthDbContext : IdentityDbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var readerRoleId = "71c6cb26-0e5f-4d65-83db-7cbf2c876a87";
            var writerRoleId = "d066c339-e4e9-4b72-bf28-a1ec864be725";

            //Create reader and writer role
            var roles = new List<IdentityRole>
            {
                new IdentityRole()
                {
                    Id = readerRoleId,
                    Name = "Reader",
                    NormalizedName = "Reader".ToUpper(),
                    ConcurrencyStamp = readerRoleId
                },
                new IdentityRole()
                {
                    Id = writerRoleId,
                    Name = "Writer",
                    NormalizedName = "Writer".ToUpper(),
                    ConcurrencyStamp = writerRoleId
                }
            };

            //Seed the roles
            builder.Entity<IdentityRole>().HasData(roles);

            //Create an Admin User
            var adminUserId = "1be0c34b-acbd-4ddb-b466-dd68b81134e7";
            var admin = new IdentityUser()
            {
                Id = adminUserId,
                UserName = "admin@blogify.com",
                NormalizedUserName = "admin@blogify.com".ToUpper(),
                Email = "admin@blogify.com",
                NormalizedEmail = "admin@blogify.com".ToUpper(),
            };

            admin.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(admin, "Admin@123");

            //Seed
            builder.Entity<IdentityUser>().HasData(admin);

            //Give Roles to Admin
            var adminRoles = new List<IdentityUserRole<string>>()
            {
                new()
                {
                    UserId = adminUserId,
                    RoleId = readerRoleId
                },
                new()
                {
                    UserId = adminUserId,
                    RoleId = writerRoleId
                }
            };
            builder.Entity<IdentityUserRole<string>>().HasData(adminRoles);
        }
    }
}
