using System.Collections.Concurrent;
using System.Numerics;

namespace Fibonacci_Core.Interfaces
{
    public interface INumericWordsConverterRepository
    {
        public ConcurrentDictionary<BigInteger, string> _cache { get; set; }
        void ResetOrChangeLanguage(string culture = "en");
    }
}