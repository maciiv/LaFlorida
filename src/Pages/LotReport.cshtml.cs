using System.Collections.Generic;
using System.Threading.Tasks;
using LaFlorida.Models;
using LaFlorida.Services;
using LaFlorida.ServicesModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace LaFlorida.Pages
{
    [Authorize(Roles = "Admin, Manager")]
    public class LotReportModel : PageModel
    {
        private readonly IReportService _reportService;
        private readonly ILotService _lotService;

        public LotReportModel(IReportService reportService, ILotService lotService)
        {
            _reportService = reportService;
            _lotService = lotService;
        }

        [BindProperty]
        public Lot Lot { get; set; }
        public IList<CycleStatistics> CycleStatistics { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            ViewData["LotId"] = await _lotService.GetLotsSelectListAsync();

            if (id != null)
            {
                Lot = await _lotService.GetLotByIdAsync((int)id);
                CycleStatistics = await _reportService.GetLotStatisticsAsync((int)id);
            }

            return Page();
        }

        public async Task<PartialViewResult> OnPostGenerateReportAsync()
        {
            CycleStatistics = await _reportService.GetLotStatisticsAsync(Lot.LotId);
            return new PartialViewResult 
            {
                ViewName = "_LotReportPartial",
                ViewData = new ViewDataDictionary<List<CycleStatistics>>(ViewData, CycleStatistics)
            };
        }
    }
}