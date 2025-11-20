using Fibonacci_Web_App.Interfaces;
using System.Numerics;

namespace Fibonacci_Web_App.Services
{
    internal static class Validation
    {
        internal static bool IsValid(string input, out BigInteger result)
        {
            if (BigInteger.TryParse(input, out result))
            {
                if (result <= 0)
                {
                    Console.WriteLine("Please enter a positive integer greater than zero.");
                    return false;
                }
                return true;
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a valid integer.");
                return false;
            }
        }
    }
}
