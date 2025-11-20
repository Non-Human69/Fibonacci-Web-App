using Fibonacci_Web_App.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Numerics;

namespace Fibonacci_Web_App.Pages.FiboPages
{
    public class FibonacciCombinationPageModel : PageModel
    {
        private readonly IFiboRepository fibonacciRepository;
        public IFiboRepository FibonacciRepository => fibonacciRepository;

        [BindProperty]
        public BigInteger inputNumber { get; set; }
        public FibonacciCombinationPageModel(IFiboRepository fibonacciRepository)
        {
            this.fibonacciRepository = fibonacciRepository;
        }

        public List<BigInteger> ShowFiboForNumber()
        {
            Console.WriteLine($"Fibonacci numbers that add up to {inputNumber}:");
            BigInteger remaining = inputNumber;
            BigInteger[] fiboNumms = fibonacciRepository.GetFibonacciNumbers();
            List<BigInteger> result = new List<BigInteger>();

            for (int i = fiboNumms.Length - 1; i >= 0; i--)
            {
                BigInteger fibo = fiboNumms[i];
                if (fibo <= remaining)
                {
                    result.Add(fibo);
                    remaining -= fibo;
                    Console.WriteLine($"Selected Fibonacci number: {fibo}, Remaining to find: {remaining}");
                }
                if (remaining == 0)
                {
                    break;
                }
            }

            if (remaining != 0)
            {
                // Could not represent the target with the provided Fibonacci numbers
                ModelState.AddModelError(string.Empty,
                    $"Cannot represent {inputNumber} with available Fibonacci numbers. Remaining: {remaining}. " +
                    "Ensure the repository provides sufficiently large Fibonacci numbers.");
                Console.WriteLine($"Could not fully represent {inputNumber}. Remaining: {remaining}");
                // Return the partial result (or you may choose to return an empty list instead)
                return result;
            }

            if (result.Count > 0)
            {
                Console.WriteLine($"Combination found: {result.Count} numbers");
                Console.WriteLine(string.Join(", ", result));
                return result;
            }
            else
            {
                Console.WriteLine("No combination found.");
                return result;
            }
        }
    }
}
