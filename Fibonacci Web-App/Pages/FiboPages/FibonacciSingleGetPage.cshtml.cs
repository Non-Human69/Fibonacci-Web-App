using Fibonacci_Core.Interfaces;
using Fibonacci_Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Numerics;

namespace Fibonacci_Web_App.Pages.FiboPages
{
    public class FibonacciSingleGetPageModel : PageModel
    {
        private readonly IFiboRepository fibonacciRepository;
        public IFiboRepository FibonacciRepository => fibonacciRepository;
        public INumericWordsConverterService service { get; }

        [BindProperty]
        public BigInteger inputNumber { get; set; }

        public FibonacciSingleGetPageModel(IFiboRepository fibonacciRepository, INumericWordsConverterService numericWordsConverterService)
        {
            this.fibonacciRepository = fibonacciRepository;
            this.service = numericWordsConverterService;
        }
    }
}
