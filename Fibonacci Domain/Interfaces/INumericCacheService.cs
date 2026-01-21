using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Fibonacci_Core.Interfaces
{
    public interface INumericCacheService
    {
        public string GetOrAdd(string culture, BigInteger key, Func<BigInteger, string> valueFactory);
    }
}
