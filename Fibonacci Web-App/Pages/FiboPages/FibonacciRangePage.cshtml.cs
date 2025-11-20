using Fibonacci_Web_App.Interfaces;
using Fibonacci_Web_App.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;
using System.Numerics;

namespace Fibonacci_Web_App.Pages.FiboPages
{
    public class FibonacciRangePageModel : PageModel
    {
        private readonly IFiboRepository fibonacciRepository;
        public IFiboRepository FibonacciRepository => fibonacciRepository;

        [BindProperty]
        public BigInteger startNumber { get; set; }
        [BindProperty]
        public BigInteger endNumber { get; set; }

        public FibonacciRangePageModel(IFiboRepository fibonacciRepository)
        {
            this.fibonacciRepository = fibonacciRepository;
        }

        public List<BigInteger> ShowFiboInRange()
        {
            if (startNumber >= endNumber)
            {
                Console.WriteLine("Starting number must be less then ending number, numbers will be swapped");
                (startNumber, endNumber) = (endNumber, startNumber);
            }
            List<BigInteger> result = FibonacciRepository.GetFibonacciNumbers().Where(f => f >= startNumber && f <= endNumber).ToList();
            Console.WriteLine($"Fibonacci numbers between {startNumber} and {endNumber}:");
            result.ForEach(item => Console.WriteLine(item));
            if(result != null) Console.WriteLine($"The sum of these fibonacci numbers is {result.Aggregate(BigInteger.Add)}");

            return result;
        }
    }
}
