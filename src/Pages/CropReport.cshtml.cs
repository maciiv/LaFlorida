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
    public class CropReportModel : PageModel
    {
        private readonly IReportService _reportService;      
        private readonly ICropService _cropService;

        public CropReportModel(IReportService reportService, ICropService cropService)
        {
            _reportService = reportService;
            _cropService = cropService;
        }

        [BindProperty]
        public Crop Crop { get; set; }
        public IList<CycleStatistics> CycleStatistics { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            ViewData["CropId"] = await _cropService.GetCropsSelectListAsync();

            if (id != null)
            {
                Crop = await _cropService.GetCropByIdAsync((int)id);
                CycleStatistics = await _reportService.GetCropStatisticsAsync((int)id);
            }

            return Page();
        }

        public async Task<PartialViewResult> OnPostGenerateReportAsync()
        {
            CycleStatistics = await _reportService.GetCropStatisticsAsync(Crop.CropId);
            return new PartialViewResult
            {
                ViewName = "_CropReportPartial",
                ViewData = new ViewDataDictionary<List<CycleStatistics>>(ViewData, CycleStatistics)
            };
        }
    }
}