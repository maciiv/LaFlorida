using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LaFlorida.Data.Configuration
{
    public class ApplicationUserRoleConfig : IEntityTypeConfiguration<IdentityUserRole<string>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
        {
            builder.HasData(
            new IdentityUserRole<string>
            {
                RoleId = "0b75fd57-659d-4ec5-9864-3be915e49a5c",
                UserId = "1030b74b-96fd-46e0-959c-4d71f99b74c7"
            },
            new IdentityUserRole<string>
            {
                RoleId = "0b75fd57-659d-4ec5-9864-3be915e49a5c",
                UserId = "0e548f75-adda-4431-9462-f113ab1adc37"
            });
        }
    }
}
