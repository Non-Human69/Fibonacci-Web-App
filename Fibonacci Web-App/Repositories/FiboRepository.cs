using Fibonacci_Web_App.Interfaces;
using System.Numerics;
using System.Collections.Generic;
namespace Fibonacci_Web_App.Repositories
{
    public class FiboRepository : IFiboRepository
    {
        private static BigInteger[] _fiboNumms = Array.Empty<BigInteger>();
        public IReadOnlyList<BigInteger> FiboNumms => _fiboNumms;
        readonly int maxCount;

        public FiboRepository(IConfiguration configuration)
        {
            maxCount = configuration?.GetValue<int>("MaxCount") ?? 100;
            if (maxCount < 0) throw new ArgumentOutOfRangeException(nameof(maxCount), "maxCount must be non-negative.");

            // Ensure numbers are loaded once when the repository instance is created
            if (_fiboNumms.Length == 0)
            {
                LoadFibonacciNumbers();
            }
        }

        public void LoadFibonacciNumbers()
        {
            if (maxCount == 1)
            {
                _fiboNumms = new BigInteger[] { 0 };
                Console.WriteLine("Generated Fibonacci List is empty");
                return;
            }

            List<BigInteger> fiboList = new List<BigInteger>(maxCount) { 0, 1 };
            for (int i = 2; i < maxCount; i++)
            {
                BigInteger nextFibo = fiboList[i - 1] + fiboList[i - 2];
                fiboList.Add(nextFibo);
                Console.WriteLine($"Generated Fibonacci number {i}: {nextFibo}");
            }
            _fiboNumms = fiboList.ToArray();
        }

        public bool CheckFibonacciNumber(BigInteger number)
        {
            if (_fiboNumms.Length == 0)
            {
                Console.WriteLine("Fibonacci numbers not loaded.");
                return false;
            }

            if (_fiboNumms.Contains(number))
            {
                Console.WriteLine($"{number} is a Fibonacci number.");
                return true;
            }
            else
            {
                Console.WriteLine($"{number} is not a Fibonacci number.");
                return false;
            }
        }

        public BigInteger GetFibonacciNumber(BigInteger i)
        {
            if (i < 0 || i >= _fiboNumms.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(i), "Index is out of range of the Fibonacci numbers list. try increasing maxCount.");
            }
            return _fiboNumms[(int)i];
        }

        public BigInteger[] GetFibonacciNumbers()
        {
            return _fiboNumms;
        }
    }
}
