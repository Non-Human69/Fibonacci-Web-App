using Fibonacci_Web_App.Interfaces;
using Fibonacci_Web_App.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Numerics;

namespace Fibonacci_Web_App.Pages.FiboPages
{
    public class FibonacciCheckerPageModel : PageModel
    {
        private readonly IFiboRepository fibonacciRepository;
        public IFiboRepository FibonacciRepository => fibonacciRepository;

        [BindProperty]
        public BigInteger inputNumber { get; set; }
        public FibonacciCheckerPageModel(IFiboRepository fibonacciRepository)
        {
            this.fibonacciRepository = fibonacciRepository;
        }
    }
}