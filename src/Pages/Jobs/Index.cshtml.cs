using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LaFlorida.Models;
using LaFlorida.Services;
using Microsoft.AspNetCore.Authorization;

namespace LaFlorida.Pages.Jobs
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly IJobService _jobService;

        public IndexModel(IJobService jobService)
        {
            _jobService = jobService;
        }

        public IList<Job> Jobs { get; set; }
        public bool Success { get; set; } = false;
        public bool Error { get; set; } = false;
        public string Message { get; set; }

        public async Task OnGetAsync(bool success, bool error, string message)
        {
            Jobs = await _jobService.GetJobsAsync();
            if (success) Success = true;
            if (error) Error = true;
            Message = message;
        }
    }
}
