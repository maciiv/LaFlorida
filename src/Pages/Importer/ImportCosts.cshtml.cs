using System.Collections.Generic;
using System.Threading.Tasks;
using LaFlorida.Helpers;
using LaFlorida.Models;
using LaFlorida.PageModels;
using LaFlorida.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace LaFlorida.Pages.Importer
{
    [Authorize(Roles = "Admin")]
    public class ImportCostsModel : PageModel
    {
        private readonly ICycleService _cycleService;
        private readonly IImportHelper _importHelper;
        private readonly ICostService _costService;
        public ImportCostsModel(ICycleService cycleService, IImportHelper importHelper, ICostService costService)
        {
            _cycleService = cycleService;
            _importHelper = importHelper;
            _costService = costService;
        }

        [BindProperty]
        public IFormFile ValidateFile { get; set; }
        [BindProperty]
        public int Id { get; set; }
        [BindProperty]
        public List<Cost> Costs { get; set; }
        public string CycleName { get; set; }
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return RedirectToPage("../Index", new { error = true, message = "Ciclo no encontrado" });
            }

            Id = (int)id;
            var cycle = await _cycleService.GetCycleByIdAsync(Id);

            if (cycle == null)
            {
                return RedirectToPage("../Index", new { error = true, message = "Ciclo no encontrado" });
            }

            CycleName = cycle.Name;

            return Page();
        }

        public async Task<PartialViewResult> OnPostValidateFileAsync(int? id)
        {
            var costs = await _importHelper.ValidateCostsFileAsync(ValidateFile, (int)id);
            var result = _importHelper.PartialModel(Id, costs);
            return new PartialViewResult
            {
                ViewName = "_ImportCostsPartial",
                ViewData = new ViewDataDictionary<_ImportPartialPageModel>(ViewData, result)
            };
        }

        public async Task<IActionResult> OnPostImportCostsAsync(int? id)
        {
            var create = await _costService.CreateBulkCostsAsync(Costs);
            if (!create.Success)
            {
                ModelState.AddModelError("error", create.Message);
                return RedirectToPage("../Dashboard", new { id, error = true, message = "Costos no importados. Por favor revise que la tabla no tenga ninguna linea en rojo" });
            }

            return RedirectToPage("../Dashboard", new { id, success = true, message = "Costos importados con exito" });
        }
    }
}
