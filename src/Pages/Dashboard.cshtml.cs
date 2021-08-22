using System.Collections.Generic;
using System.Threading.Tasks;
using LaFlorida.Models;
using LaFlorida.Services;
using LaFlorida.ServicesModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LaFlorida.Pages
{
    [Authorize(Roles = "Admin, Manager")]
    public class DashboardModel : PageModel
    {
        private readonly ICostService _costService;      
        private readonly ISaleService _saleService;
        private readonly ICycleService _cycleService;
        private readonly IReportService _reportService;

        public DashboardModel(ICostService costService, ISaleService saleService, ICycleService cycleService, IReportService reportService)
        {
            _costService = costService;
            _saleService = saleService;
            _cycleService = cycleService;
            _reportService = reportService;
        }

        public IList<Cost> Costs { get; set; }
        public IList<Sale> Sales { get; set; }
        public Cycle Cycle { get; set; }
        public IList<CycleCostByUser> CycleCostsByUsers { get; set; }
        public CycleCostByUser MachinistCosts { get; set; }
        public bool IsComplete { get; set; }
        public bool IsRent { get; set; }
        public CycleStatistics CycleStatistics { get; set; }
        public bool Success { get; set; } = false;
        public bool Error { get; set; } = false;
        public string Message { get; set; }
        [BindProperty]
        public int Id { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id, bool success, bool error, string message)
        {
            if (id == null)
            {
                return RedirectToPage("./CurrentCycles", new { error = true, message = "Ciclo no encontrado" });
            }

            Cycle = await _cycleService.GetCycleByIdAsync((int)id);

            if (Cycle == null)
            {
                return RedirectToPage("./CurrentCycles", new { error = true, message = "Ciclo no encontrado" });
            }

            Id = (int)id;
            Costs = await _costService.GetCostsByCycleAsync((int)id);
            Sales = await _saleService.GetSalesByCycleAsync((int)id);
            CycleCostsByUsers = await _reportService.GetCycleCostByUsersAsync((int)id);
            CycleStatistics = await _reportService.GetCycleStatisticsAsync((int)id);
            MachinistCosts = await _reportService.GetCycleMachinistCostAsync((int)id);

            IsComplete = Cycle.IsComplete;
            IsRent = Cycle.IsRent;

            if (success) Success = true;
            if (error) Error = true;
            Message = message;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Id == 0)
            {
                return RedirectToPage("./CurrentCycles", new { error = true, message = "Ciclo no encontrado" });
            }

            var close = await _cycleService.CloseCycleAsync(Id);
            if (!close.Success)
            {
                ModelState.AddModelError("error", close.Message);
                return Page();
            }

            return RedirectToPage("./CurrentCycles", new { success = true, message = "Ciclo cerrado con exito" });
        }
    }
}