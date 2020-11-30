using LaFlorida.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LaFlorida.Data
{
    public interface IApplicationDbContext
    {
        DbSet<ApplicationUser> ApplicationUsers { get; set; }
        DbSet<IdentityRole> ApplicationRoles { get; set; }
        DbSet<Cost> Costs { get; set; }
        DbSet<Crop> Crops { get; set; }
        DbSet<Cycle> Cycles { get; set; }
        DbSet<Job> Jobs { get; set; }
        DbSet<Lot> Lots { get; set; }
        DbSet<Sale> Sales { get; set; }
        DbSet<Withdraw> Withdraws { get; set; }

        ValueTask<EntityEntry> AddAsync(object entity, CancellationToken cancellationToken = default);
        Task AddRangeAsync(IEnumerable<object> entities, CancellationToken cancellationToken = default);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        EntityEntry Attach(object entity);
    }
}