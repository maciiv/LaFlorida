using LaFlorida.Models;
using LaFlorida.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace LaFlorida.PageModels
{
    public class SalesPageModel : PageModel
    {
        public ISaleService _saleService;
        private readonly ICycleService _cycleService;

        public SalesPageModel(ISaleService saleService, ICycleService cycleService)
        {
            _saleService = saleService;
            _cycleService = cycleService;
        }

        public string CycleName { get; set; }

        public async Task<Sale> HasCycle(int? cycleId)
        {
            CycleName = (await _cycleService.GetCycleByIdAsync((int)cycleId))?.Name;
            return new Sale
            {
                CycleId = (int)cycleId
            };
        }

        public async Task SetSelectLists()
        {
            ViewData["CycleId"] = await _cycleService.GetCyclesSelectListAsync();
        }
    }
}
