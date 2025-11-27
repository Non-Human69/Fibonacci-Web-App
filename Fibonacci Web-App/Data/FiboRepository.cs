using Fibonacci_Web_App.Interfaces;
using System.Numerics;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System;

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
            if (maxCount <= 0)
            {
                _fiboNumms = Array.Empty<BigInteger>();
                Console.WriteLine("Generated Fibonacci List is empty");
                return;
            }

            // Pre-allocate and compute in linear time (fastest for full-range generation)
            var fiboArray = new BigInteger[maxCount];
            fiboArray[0] = BigInteger.Zero;
            if (maxCount > 1)
            {
                fiboArray[1] = BigInteger.One;
                for (int i = 2; i < maxCount; i++)
                {
                    // Single BigInteger addition per step
                    fiboArray[i] = fiboArray[i - 1] + fiboArray[i - 2];
                }
            }

            _fiboNumms = fiboArray;

            // Log one summary (avoid logging each element)
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
