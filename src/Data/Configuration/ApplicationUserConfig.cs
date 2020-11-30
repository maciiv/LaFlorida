using LaFlorida.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LaFlorida.Data.Configuration
{
    public class ApplicationUserConfig : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            var hasher = new PasswordHasher<ApplicationUser>();

            builder.HasData(
                new ApplicationUser
                {
                    Id = "1030b74b-96fd-46e0-959c-4d71f99b74c7",
                    UserName = "m.canizares@outlook.com",
                    NormalizedUserName = "M.CANIZARES@OUTLOOK.COM",
                    Email = "m.canizares@outlook.com",
                    NormalizedEmail = "M.CANIZARES@OUTLOOK.COM",
                    EmailConfirmed = true,
                    PasswordHash = hasher.HashPassword(null, "LaFlorida89"),
                    PhoneNumber = "0400157444",
                    PhoneNumberConfirmed = true,
                    TwoFactorEnabled = false,
                    LockoutEnabled = false,
                    AccessFailedCount = 0,
                    FirstName = "Miguel",
                    LastName = "Canizares",
                },
                new ApplicationUser
                {
                    Id = "0e548f75-adda-4431-9462-f113ab1adc37",
                    UserName = "jorlcm@hotmail.com",
                    NormalizedUserName = "JORLCM@HOTMAIL.COM",
                    Email = "jorlcm@hotmail.com",
                    NormalizedEmail = "JORLCM@HOTMAIL.COM",
                    EmailConfirmed = true,
                    PasswordHash = hasher.HashPassword(null, "LaFlorida83"),
                    PhoneNumber = "0998229186",
                    PhoneNumberConfirmed = true,
                    TwoFactorEnabled = false,
                    LockoutEnabled = false,
                    AccessFailedCount = 0,
                    FirstName = "Jorge",
                    LastName = "Canizares",
                });
        }
    }
}
