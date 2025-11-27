using Fibonacci_Web_App.Options;
using System.Collections.Concurrent;
using System.Numerics;

namespace Fibonacci_Web_App.Interfaces
{
    public interface INumericWordsConverterRepository
    {
        public ConcurrentDictionary<BigInteger, string> _cache { get; set; }
        void ResetOrChangeLanguage(string culture = "en");
    }
}