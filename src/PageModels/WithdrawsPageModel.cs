using LaFlorida.Models;
using LaFlorida.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace LaFlorida.PageModels
{
    public class WithdrawsPageModel : PageModel
    {
        public IWithdrawService _withdrawService;
        private readonly IApplicationUserService _applicationUserService;
        private readonly ICycleService _cycleService;

        public WithdrawsPageModel(IWithdrawService withdrawService, IApplicationUserService applicationUserService, ICycleService cycleService)
        {
            _withdrawService = withdrawService;
            _applicationUserService = applicationUserService;
            _cycleService = cycleService;
        }

        public string CycleName { get; set; }
        public string ApplicationUserName { get; set; }

        public async Task<Withdraw> HasCycle(int? cycleId, string applicationUserId)
        {
            CycleName = (await _cycleService.GetCycleByIdAsync((int)cycleId))?.Name;
            var applicationUser = await _applicationUserService.GetApplicationUserByIdAsync(applicationUserId);
            ApplicationUserName = applicationUser == null ? null : $"{applicationUser?.FirstName} {applicationUser?.LastName}";

           return new Withdraw
            {
                ApplicationUserId = applicationUserId,
                CycleId = (int)cycleId
            };
        }
        public async Task SetSelectLists()
        {
            ViewData["ApplicationUserId"] = await _applicationUserService.GetApplicationUsersSelectListAsync();
            ViewData["CycleId"] = await _cycleService.GetCyclesSelectListAsync();
        }
    }
}
