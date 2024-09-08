using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;

namespace LaFlorida.Components.Bases
{
    [Authorize]
    [StreamRendering]
    public class LaFloridaComponent : ComponentBase
    {
        [Inject]
        public required NavigationManager Nav { get; set; }
        [SupplyParameterFromQuery]
        public bool Success { get; set; } = false;
        [SupplyParameterFromQuery]
        public bool Error { get; set; } = false;
        [SupplyParameterFromQuery]
        public string Message { get; set; }
    }
}
