using LaFlorida.Models;
using LaFlorida.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace LaFlorida.PageModels
{
    public class CostsPageModel : PageModel
    {
        public ICostService _costService;
        private readonly ICycleService _cycleService;
        private readonly IJobService _jobService;
        private readonly IApplicationUserService _applicationUserService;

        public CostsPageModel(ICostService costService, ICycleService cycleService, IJobService jobService, IApplicationUserService applicationUserService)
        {
            _costService = costService;
            _cycleService = cycleService;
            _jobService = jobService;
            _applicationUserService = applicationUserService;
        }

        public string CycleName { get; set; }

        public async Task SetSelectLists()
        {
            ViewData["ApplicationUserId"] = await _applicationUserService.GetApplicationUsersSelectListAsync();
            ViewData["CycleId"] = await _cycleService.GetCyclesSelectListAsync();
            ViewData["JobId"] = await _jobService.GetJobsSelectListAsync();
        }

        public async Task<Cost> HasCycle(int? cycleId)
        {
            CycleName = (await _cycleService.GetCycleByIdAsync((int)cycleId))?.Name;
            return new Cost
            {
                CycleId = (int)cycleId
            };
        }
    }
}
