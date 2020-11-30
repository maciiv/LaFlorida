using System.Collections.Generic;
using System.Threading.Tasks;
using LaFlorida.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LaFlorida.Pages.ApplicationRoles
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly IApplicationRoleService _applicationRoleService;

        public IndexModel(IApplicationRoleService applicationRoleService)
        {
            _applicationRoleService = applicationRoleService;
        }

        public IList<IdentityRole> ApplicationRoles { get; set; }
        public bool Success { get; set; } = false;
        public bool Error { get; set; } = false;
        public string Message { get; set; }

        public async Task OnGetAsync(bool success, bool error, string message)
        {
            ApplicationRoles = await _applicationRoleService.GetApplicationRolesAsync();
            if (success) Success = true;
            if (error) Error = true;
            Message = message;
        }
    }
}