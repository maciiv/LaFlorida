using LaFlorida.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LaFlorida.Services
{
    public interface IApplicationRoleService
    {
        Task<IdentityResult> CreateApplicationRoleAsync(string role);
        Task<IdentityResult> EditApplicationRoleAsync(string id, string role);
        Task<IdentityResult> DeleteApplicationRoleAsync(string id);
        Task<List<IdentityRole>> GetApplicationRolesAsync();
        Task<IdentityRole> GetApplicationRoleByIdAsync(string id);
    }

    public class ApplicationRoleService : IApplicationRoleService
    {
        private readonly IApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ApplicationRoleService(RoleManager<IdentityRole> roleManager, IApplicationDbContext context)
        {
            _roleManager = roleManager;
            _context = context;
        }

        public async Task<IdentityResult> CreateApplicationRoleAsync(string role)
        {
            var roleExists = await _roleManager.RoleExistsAsync(role);

            if (roleExists) return new IdentityResult();

            var newRole = new IdentityRole
            {
                Name = role
            };

            return await _roleManager.CreateAsync(newRole);
        }

        public async Task<IdentityResult> EditApplicationRoleAsync(string id, string role)
        {
            var existingRole = await _roleManager.FindByIdAsync(id);

            existingRole.Name = role;

            return await _roleManager.UpdateAsync(existingRole);
        }

        public async Task<IdentityResult> DeleteApplicationRoleAsync(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            return await _roleManager.DeleteAsync(role);
        }

        public async Task<List<IdentityRole>> GetApplicationRolesAsync()
        {
            return await _context.ApplicationRoles.ToListAsync();
        }

        public async Task<IdentityRole> GetApplicationRoleByIdAsync(string id)
        {
            return await _roleManager.FindByIdAsync(id);
        }
    }
}
