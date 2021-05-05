using LaFlorida.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace LaFlorida.PageModels
{
    public class _ImportPartialPageModel : PageModel
    {
        [BindProperty]
        public List<Cost> Costs { get; set; }
        [BindProperty]
        public List<Sale> Sales { get; set; }
        [BindProperty]
        public int Id { get; set; }

        public bool ValidateRow(Cost cost)
        {
            return cost.Cycle != null && cost.Job != null && cost.ApplicationUser != null && cost.Quantity > 0;
        }
    }
}
