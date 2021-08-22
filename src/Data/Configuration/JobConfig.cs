using LaFlorida.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LaFlorida.Data.Configuration
{
    public class JobConfig : IEntityTypeConfiguration<Job>
    {
        public void Configure(EntityTypeBuilder<Job> builder)
        {
            builder.HasData(
            new Job
            {
                JobId = 1,
                Name = "Maquinaria",
                IsMachinist = true,
                IsRent = true
            },
            new Job
            {
                JobId = 2,
                Name = "Arriendo",
                IsRent = true
            },
            new Job
            {
                JobId = 3,
                Name = "Semilla"
            },
            new Job
            {
                JobId = 4,
                Name = "Fertilizante"
            },
            new Job
            {
                JobId = 5,
                Name = "Mano de Obra"
            },
            new Job
            {
                JobId = 6,
                Name = "Quimicos"
            });
        }
    }
}
