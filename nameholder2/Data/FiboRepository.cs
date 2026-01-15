using Microsoft.Extensions.Configuration;
using System.Numerics;
using Fibonacci_Core.Interfaces;
using Microsoft.Extensions.Primitives; // Required for IConfiguration
using System.Linq; // For .Contains

namespace Fibonacci_Infrastructure.Data
{
    public class FiboRepository : IFiboRepository
    {
        private static BigInteger[] _fiboNumms = Array.Empty<BigInteger>();
        public IReadOnlyList<BigInteger> FiboNumms => _fiboNumms;
        readonly int maxCount;

        public FiboRepository(IConfiguration configuration)
        {
            // FIX: Use ConfigurationBinder.GetValue<T> directly with the correct using
            maxCount = configuration.GetValue<int>("MaxCount", 100);
            if (maxCount < 0) throw new ArgumentOutOfRangeException(nameof(maxCount), "maxCount must be non-negative.");

            if (_fiboNumms.Length == 0)
            {
                LoadFibonacciNumbers();
            }
        }

        public void LoadFibonacciNumbers()
        {
            if (maxCount <= 0)
            {
                _fiboNumms = Array.Empty<BigInteger>();
                Console.WriteLine("Generated Fibonacci List is empty");
                return;
            }

            var fiboArray = new BigInteger[maxCount];
            fiboArray[0] = BigInteger.Zero;
            if (maxCount > 1)
            {
                fiboArray[1] = BigInteger.One;
                for (int i = 2; i < maxCount; i++)
                {
                    fiboArray[i] = fiboArray[i - 1] + fiboArray[i - 2];
                }
            }

            _fiboNumms = fiboArray;

            Console.WriteLine($"Generated {maxCount} Fibonacci numbers. Last index: {maxCount - 1}, Last value length (digits): {_fiboNumms[^1].ToString().Length}");
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
