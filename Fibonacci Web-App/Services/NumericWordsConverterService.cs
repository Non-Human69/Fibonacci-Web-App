using Fibonacci_Web_App.Interfaces;
using Fibonacci_Web_App.Options;
using System.Numerics;
using System.Text;
using System.Globalization;
using System.Linq;
using Fibonacci_Web_App.Providers;

namespace Fibonacci_Web_App.Services
{
    public class NumericWordsConverterService
    {
        private readonly NumericCacheService _cache;
        private readonly NumericData _numericData;
        private readonly List<(ScaleItem Item, BigInteger Value)> _scaleValues;

        public NumericWordsConverterService(NumericDataProvider provider, NumericCacheService numericCacheService)
        {
            _cache = numericCacheService;

            var culture = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
            _numericData = provider.GetForCulture(culture);

            var scaleValues = new List<(ScaleItem Item, BigInteger Value)>();

            if (_numericData?.unnamed != null)
            {
                scaleValues.AddRange(_numericData.unnamed
                    .Select(s => (Item: s, Value: BigInteger.Pow(10, s.Power)))
                    .OrderByDescending(t => t.Item.Power));
            }

            if (_numericData?.Rest != null)
            {
                scaleValues.AddRange(_numericData.Rest
                    .Select(s => (Item: s, Value: BigInteger.Pow(10, s.Power)))
                    .OrderByDescending(t => t.Item.Power));
            }

            _scaleValues = scaleValues;
        }
                
        public string ConvertToWords(BigInteger i)
        {
            var culture = System.Globalization.CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
            return _cache.GetOrAdd(culture, i, key =>
            {
                var sb = new StringBuilder();
                AppendWords(key, sb);
                return sb.ToString().Trim();
            });
        }

        private void AppendWords(BigInteger i, StringBuilder sb)
        {
            if (i == 0)
            {
                if (sb.Length == 0)
                    sb.Append(_numericData.ZeroTo99[0]);
                else
                    sb.Append(' ').Append(_numericData.ZeroTo99[0]);
                return;
            }

            if (i < 100)
            {
                if (sb.Length > 0) sb.Append(' ');
                sb.Append(_numericData.ZeroTo99[(int)i]);
                return;
            }

            if (i < 1000)
            {
                BigInteger hundreds = i / 100;
                BigInteger remainder = i % 100;

                if (sb.Length > 0) sb.Append(' ');
                sb.Append(_numericData.ZeroTo99[(int)hundreds]);
                sb.Append(' ').Append(_numericData.Hundred);

                if (remainder > 0)
                {
                    sb.Append(' ');
                    AppendWords(remainder, sb);
                }
                return;
            }

            if (_scaleValues.Count > 0 && i >= BigInteger.Pow(10, _scaleValues.First().Item.Power + 1))
            {
                if (sb.Length > 0) sb.Append(' ');
                sb.Append("Too big of a number detected");
                return;
            }

            foreach (var (Item, Value) in _scaleValues)
            {
                if (i >= Value)
                {
                    BigInteger leading = i / Value;
                    BigInteger remainder = i % Value;

                    if (sb.Length > 0) sb.Append(' ');
                    AppendWords(leading, sb);
                    sb.Append(' ').Append(Item.Name);

                    if (remainder > 0)
                    {
                        sb.Append(' ');
                        AppendWords(remainder, sb);
                    }
                    return;
                }
            }

            if (sb.Length > 0) sb.Append(' ');
            sb.Append(i.ToString());
        }
    }
}
