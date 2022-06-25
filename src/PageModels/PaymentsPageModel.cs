using LaFlorida.Models;
using LaFlorida.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace LaFlorida.PageModels
{
    public class PaymentsPageModel : PageModel
    {
        public IPaymentService _paymentService;
        private readonly IApplicationUserService _applicationUserService;
        private readonly ICycleService _cycleService;

        public PaymentsPageModel(IPaymentService paymentService, IApplicationUserService applicationUserService, ICycleService cycleService)
        {
            _paymentService = paymentService;
            _applicationUserService = applicationUserService;
            _cycleService = cycleService;
        }

        public string CycleName { get; set; }
        public string ApplicationUserName { get; set; }

        public async Task<Payment> HasCycle(int? cycleId, string applicationUserId)
        {
            CycleName = (await _cycleService.GetCycleByIdAsync((int)cycleId))?.Name;
            var applicationUser = await _applicationUserService.GetApplicationUserByIdAsync(applicationUserId);
            ApplicationUserName = applicationUser == null ? null : $"{applicationUser?.FirstName} {applicationUser?.LastName}";

           return new Payment
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
