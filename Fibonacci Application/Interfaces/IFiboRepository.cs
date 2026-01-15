using System.Numerics;
using System.Collections.Generic;

namespace Fibonacci_Application.Interfaces
{
    public interface IFiboRepository
    {
        IReadOnlyList<BigInteger> FiboNumms { get; }
        void LoadFibonacciNumbers();
        bool CheckFibonacciNumber(BigInteger number);
        BigInteger GetFibonacciNumber(BigInteger i);
        BigInteger[] GetFibonacciNumbers();
    }
}