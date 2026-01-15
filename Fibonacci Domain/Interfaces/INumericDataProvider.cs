using Fibonacci_Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fibonacci_Core.Interfaces
{
    public interface INumericDataProvider
    {
        public void ClearCache();

        public NumericData GetForCulture(string culture);
    }
}
