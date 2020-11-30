using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LaFlorida.Data.Configuration
{
    public class ApplicationRoleConfig : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(
                new IdentityRole
                {
                    Id = "0b75fd57-659d-4ec5-9864-3be915e49a5c",
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                 new IdentityRole
                 {
                     Id = "cd074895-bf44-40d6-b511-61848932ad64",
                     Name = "Manager",
                     NormalizedName = "MANAGER"
                 },
                 new IdentityRole
                 {
                     Id = "e0219917-9bb9-4433-8f1b-123246352e99",
                     Name = "Client",
                     NormalizedName = "CLIENT"
                 },
                 new IdentityRole
                 {
                     Id = "f23e58df-3f4a-49c6-9b28-a9043cfe0557",
                     Name = "Machinist",
                     NormalizedName = "MACHINIST"
                 }
            );
        }
    }
}
