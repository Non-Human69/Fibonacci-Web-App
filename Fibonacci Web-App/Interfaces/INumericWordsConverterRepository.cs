using Fibonacci_Web_App.Models;
using System.Numerics;

namespace Fibonacci_Web_App.Interfaces
{
    public interface INumericWordsConverterRepository
    {
        string ConvertToWords(BigInteger i);
        void ResetOrChangeLanguage();
    }
}