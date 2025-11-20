using Fibonacci_Web_App.Models;
using Fibonacci_Web_App.Repositories;
using System.Collections.Concurrent;
using System.Numerics;
using System.Text;

namespace Fibonacci_Web_App.Services
{
    public class NumericWordsConverterService
    {
        private NumericData _numericData;
        private List<(ScaleItem Item, BigInteger Value)> _scaleValues;
        private readonly ConcurrentDictionary<BigInteger, string> _cache;
        public NumericWordsConverterService(NumericData numericData, List<(ScaleItem Item, BigInteger Value)> scaleValues, ConcurrentDictionary<BigInteger, string> cache)
        {
            _numericData = numericData;
            _scaleValues = scaleValues;
            _cache = cache;
        }

        public string ConvertToWords(BigInteger i)
        {
            // Use cache to reduce repeated work for the same value
            return _cache.GetOrAdd(i, key =>
            {
                var sb = new StringBuilder();
                AppendWords(key, sb);
                return sb.ToString().Trim();
            });
        }

        // Append textual representation of non-negative BigInteger 'i' to StringBuilder 'sb'.
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

            // Properly handle hundreds (100..999) by composing "<leading> hundred [remainder]"
            if (i < 1000)
            {
                BigInteger hundreds = i / 100;
                BigInteger remainder = i % 100;

                if (sb.Length > 0) sb.Append(' ');
                // e.g., "one" + " hundred"
                sb.Append(_numericData.ZeroTo99[(int)hundreds]);
                sb.Append(' ').Append(_numericData.Hundred);

                if (remainder > 0)
                {
                    sb.Append(' ');
                    Console.WriteLine($"remaining to find {remainder}");
                    AppendWords(remainder, sb);
                }
                return;
            }
            // Stops numbers that go beyond the largest possible scale
            if (i >= BigInteger.Pow(10, _scaleValues.First().Item.Power + 1))
            {
                if (sb.Length > 0) sb.Append(' ');
                sb.Append("To big of a number detected");
                return;
            }

            // Handle larger scales using precomputed scale values
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
                        Console.WriteLine($"remaining to find out {remainder}");
                        sb.Append(' ');
                        AppendWords(remainder, sb);
                    }
                    return;
                }
            }

            // Fallback: append numeric string if nothing matches (shouldn't normally occur)
            if (sb.Length > 0) sb.Append(' ');
            Console.WriteLine("error value not handeled");
            sb.Append(i.ToString());
        }
    }
}
