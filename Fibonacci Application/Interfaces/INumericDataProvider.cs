using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fibonacci_Domain.Entities;

namespace Fibonacci_Application.Interfaces
{
    public interface INumericDataProvider
    {
        public NumericData GetForCulture(string culture) => (culture ?? "en").Split('-')[0].ToUpperInvariant();
        public void ClearCache();
    }
}
