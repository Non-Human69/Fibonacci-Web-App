using System.Collections.Concurrent;
using System.Numerics;

namespace Fibonacci_Web_App.Services
{
    public class NumericCacheService
    {
        // composite key "{culture}|{number}"
        private readonly ConcurrentDictionary<string, string> _cache;
        public NumericCacheService()
        {
            _cache = new ConcurrentDictionary<string, string>();
        }

        public string GetOrAdd(string culture, BigInteger key, Func<BigInteger, string> valueFactory)
        {
            var compositeKey = $"{culture}|{key}";
            return _cache.GetOrAdd(compositeKey, _ => valueFactory(key));
        }
    }
}
