using Fibonacci_Web_App.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Numerics;

namespace Fibonacci_Web_App.Pages.FiboPages
{
    public class FibonacciSingleGetPageModel : PageModel
    {
        private readonly IFiboRepository fibonacciRepository;
        public IFiboRepository FibonacciRepository => fibonacciRepository;
        public INumericWordsConverterRepository NumericWordsConverterRepository { get; }

        [BindProperty]
        public BigInteger inputNumber { get; set; }

        public FibonacciSingleGetPageModel(IFiboRepository fibonacciRepository, INumericWordsConverterRepository numericWordsConverterRepository)
        {
            this.fibonacciRepository = fibonacciRepository;
            this.NumericWordsConverterRepository = numericWordsConverterRepository;
        }   
    }
}
