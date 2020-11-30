using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LaFlorida.Models;
using LaFlorida.Services;
using Microsoft.AspNetCore.Authorization;

namespace LaFlorida.Pages.Withdraws
{
    [Authorize(Roles = "Admin, Manager")]
    public class IndexModel : PageModel
    {
        private readonly IWithdrawService _withdrawService;

        public IndexModel(IWithdrawService withdrawService)
        {
            _withdrawService = withdrawService;
        }

        public IList<Withdraw> Withdraw { get;set; }
        public bool Success { get; set; } = false;
        public bool Error { get; set; } = false;
        public string Message { get; set; }

        public async Task OnGetAsync(bool success, bool error, string message)
        {
            Withdraw = await _withdrawService.GetWithdrawsAsync();
            if (success) Success = true;
            if (error) Error = true;
            Message = message;
        }
    }
}
