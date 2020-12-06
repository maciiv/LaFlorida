using LaFlorida.Data;
using LaFlorida.Models;
using LaFlorida.ServicesModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaFlorida.Services
{
    public interface IApplicationUserService
    {
        Task<IdentityResult> CreateApplicationUserAsync(ApplicationUserCreate register);
        Task<IdentityResult> EditApplicationUserAsync(ApplicationUserEdit applicationUser);
        Task<IdentityResult> EditApplicationUserBaseAsync(ApplicationUserBase applicationUser);
        Task<IdentityResult> DeleteApplicationUserAsync(string id);
        Task<List<ApplicationUser>> GetApplicationUsersAsync();
        Task<ApplicationUser> GetApplicationUserByIdAsync(string id);
        Task<ApplicationUserEdit> GetRegisterApplicationUserByIdAsync(string id);
        Task<ApplicationUserBase> GetRegisterApplicationUserByNameAsync(string name);
        Task<IdentityResult> ConfirmEmailAsync(string id);
        Task<IdentityResult> LockoutUserAsync(string id);
        Task<IdentityResult> LockinUserAsync(string id);
        Task<IdentityResult> RemoveRoleAsync(string id, string rol);
        Task<IdentityResult> ResetPasswordAsync(ApplicationUserResetPassword applicationUser);
        Task<SelectList> GetApplicationUsersSelectListAsync();
    }

    public class ApplicationUserService : IApplicationUserService
    {
        private readonly IApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ApplicationUserService(IApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IdentityResult> CreateApplicationUserAsync(ApplicationUserCreate register)
        {
            var user = new ApplicationUser
            {
                FirstName = register.FirstName,
                LastName = register.LastName,
                Email = register.Email,
                UserName = register.Email
            };

            var result = await _userManager.CreateAsync(user, register.Password);
            if (string.IsNullOrEmpty(register.RoleId)) return result;

            var newUser = await _userManager.FindByEmailAsync(user.Email);
            if (newUser == null) return result;
            if (await _userManager.IsInRoleAsync(newUser, register.RoleId)) return result;

            var addToRole = await _userManager.AddToRoleAsync(newUser, register.RoleId);
            return addToRole;
        }

        public async Task<IdentityResult> EditApplicationUserAsync(ApplicationUserEdit applicationUser)
        {
            var user = await _userManager.FindByIdAsync(applicationUser.Id);

            if (user == null) return new IdentityResult ();

            var result = await EditApplicationUserBaseAsync(applicationUser);

            if (await _userManager.IsInRoleAsync(user, applicationUser.RoleId)) return result;

            var addToRole = await _userManager.AddToRoleAsync(user, applicationUser.RoleId);
            return addToRole;
        }

        public async Task<IdentityResult> EditApplicationUserBaseAsync(ApplicationUserBase applicationUser)
        {
            var user = await _userManager.FindByIdAsync(applicationUser.Id);

            if (user == null) return new IdentityResult();

            user.FirstName = applicationUser.FirstName;
            user.LastName = applicationUser.LastName;
            return await _userManager.UpdateAsync(user);
        }

        public async Task<IdentityResult> DeleteApplicationUserAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            
            if (user == null) return new IdentityResult();

            return await _userManager.DeleteAsync(user);
        }

        public async Task<List<ApplicationUser>> GetApplicationUsersAsync()
        {
            return await _context.ApplicationUsers.ToListAsync();
        }

        public async Task<ApplicationUser> GetApplicationUserByIdAsync(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        public async Task<ApplicationUserEdit> GetRegisterApplicationUserByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null) return new ApplicationUserEdit();
            var roles = await _userManager.GetRolesAsync(user);

            return new ApplicationUserEdit
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                RoleId = roles.FirstOrDefault(),
                Roles = (List<string>)roles
            };
        }

        public async Task<ApplicationUserBase> GetRegisterApplicationUserByNameAsync(string name)
        {
            var user = await _userManager.FindByNameAsync(name);

            if (user == null) return new ApplicationUserBase();
            var roles = await _userManager.GetRolesAsync(user);

            return new ApplicationUserBase
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName
            };
        }

        public async Task<IdentityResult> ConfirmEmailAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            return await _userManager.ConfirmEmailAsync(user, token);
        }

        public async Task<IdentityResult> LockoutUserAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            await _userManager.SetLockoutEnabledAsync(user, true);
            return await _userManager.SetLockoutEndDateAsync(user, DateTime.Now.AddYears(10));
        }

        public async Task<IdentityResult> LockinUserAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            await _userManager.SetLockoutEnabledAsync(user, true);
            return await _userManager.SetLockoutEndDateAsync(user, null);
        }

        public async Task<IdentityResult> RemoveRoleAsync(string id, string rol)
        {
            var user = await _userManager.FindByIdAsync(id);
            return await _userManager.RemoveFromRoleAsync(user, rol);
        }

        public async Task<IdentityResult> ResetPasswordAsync(ApplicationUserResetPassword applicationUser)
        {
            var user = await _userManager.FindByIdAsync(applicationUser.Id);
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            return await _userManager.ResetPasswordAsync(user, token, applicationUser.Password);
        }

        public async Task<SelectList> GetApplicationUsersSelectListAsync()
        {
            var usersList = await GetApplicationUsersAsync();
            return new SelectList(usersList.Where(c => c.EmailConfirmed && c.LockoutEnd == null).Select(c => new { c.Id, Name = $"{c.FirstName} {c.LastName}" }).OrderBy(c => c.Name), "Id", "Name");
        }
    }
}
