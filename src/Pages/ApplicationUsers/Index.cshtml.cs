using System.Collections.Generic;
using System.Threading.Tasks;
using LaFlorida.Models;
using LaFlorida.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace LaFlorida.Pages.ApplicationUsers
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly IApplicationUserService _applicationUserService;

        public IndexModel(IApplicationUserService applicationUserService)
        {
            _applicationUserService = applicationUserService;
        }

        public IList<ApplicationUser> ApplicationUsers { get; set; }
        public bool Success { get; set; } = false;
        public bool Error { get; set; } = false;
        public string Message { get; set; }

        public async Task OnGetAsync(bool success, bool error, string message)
        {
            ApplicationUsers = await _applicationUserService.GetApplicationUsersAsync();
            if (success) Success = true;
            if (error) Error = true;
            Message = message;
        }

        public async Task<PartialViewResult> OnGetConfirmEmailAsync(string id)
        {
            var result = await _applicationUserService.ConfirmEmailAsync(id);
            ApplicationUsers = await _applicationUserService.GetApplicationUsersAsync();
            return new PartialViewResult
            {
                ViewName = "_IndexPartial",
                ViewData = new ViewDataDictionary<List<ApplicationUser>>(ViewData, ApplicationUsers)
            };
        }

        public async Task<PartialViewResult> OnGetLockoutUserAsync(string id)
        {
            var result = await _applicationUserService.LockoutUserAsync(id);
            ApplicationUsers = await _applicationUserService.GetApplicationUsersAsync();
            return new PartialViewResult
            {
                ViewName = "_IndexPartial",
                ViewData = new ViewDataDictionary<List<ApplicationUser>>(ViewData, ApplicationUsers)
            };
        }

        public async Task<PartialViewResult> OnGetLockinUserAsync(string id)
        {
            var result = await _applicationUserService.LockinUserAsync(id);
            ApplicationUsers = await _applicationUserService.GetApplicationUsersAsync();
            return new PartialViewResult
            {
                ViewName = "_IndexPartial",
                ViewData = new ViewDataDictionary<List<ApplicationUser>>(ViewData, ApplicationUsers)
            };
        }
    }
}