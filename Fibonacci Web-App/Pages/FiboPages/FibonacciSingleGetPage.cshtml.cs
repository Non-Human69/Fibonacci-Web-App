using Fibonacci_Web_App.Interfaces;
using Fibonacci_Web_App.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Numerics;

namespace Fibonacci_Web_App.Pages.FiboPages
{
    public class FibonacciSingleGetPageModel : PageModel
    {
        private readonly IFiboRepository fibonacciRepository;
        public IFiboRepository FibonacciRepository => fibonacciRepository;
        public NumericWordsConverterService service { get; }

        [BindProperty]
        public BigInteger inputNumber { get; set; }

        public FibonacciSingleGetPageModel(IFiboRepository fibonacciRepository, NumericWordsConverterService numericWordsConverterService)
        {
            this.fibonacciRepository = fibonacciRepository;
            this.service = numericWordsConverterService;
        }
    }
}
