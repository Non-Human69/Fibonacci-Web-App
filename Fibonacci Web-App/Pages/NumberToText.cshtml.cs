using Fibonacci_Core.Interfaces;
using Fibonacci_Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Numerics;

namespace Fibonacci_Web_App.Pages
{
    public class NumberToTextModel : PageModel
    {
        private readonly INumericWordsConverterService _service;

        [BindProperty]
        public BigInteger InputNumber { get; set; }

        // Expose for view if needed (read-only)
        public INumericWordsConverterService Service => _service;

        public NumberToTextModel(INumericWordsConverterService numericWordsConverterService)
        {
            _service = numericWordsConverterService;
        }

        // Ensure form posts re-render the page (handler name matches asp-page-handler="Check")
        public IActionResult OnPostCheck()
        {
            // Model binding has populated InputNumber and DI provided '_service'
            return Page();
        }
    }
}