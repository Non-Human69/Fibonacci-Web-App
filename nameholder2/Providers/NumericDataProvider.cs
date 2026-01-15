using Fibonacci_Core.Entities;
using Fibonacci_Core.Interfaces;
using Microsoft.Extensions.Hosting;
using System.Collections.Concurrent;
using System.IO;
using System.Text.Json;

namespace Fibonacci_Infrastructure.Providers
{
    public class NumericDataProvider : INumericDataProvider
    {
        private readonly ConcurrentDictionary<string, NumericData> _cache = new();
        private readonly string _resourcesPath;
        private readonly NumericData _fallback;
         
        public NumericDataProvider(IHostEnvironment env)
        {
            _resourcesPath = Path.Combine(env.ContentRootPath, "Resources");

            var fallbackFile = Path.Combine(_resourcesPath, "NumericData_EN.json");
            if (File.Exists(fallbackFile))
            {
                var json = File.ReadAllText(fallbackFile);
                _fallback = JsonSerializer.Deserialize<NumericData>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? throw new InvalidDataException("Failed to load data");
            }
        }

        private static string NormalizeCulture(string culture) => (culture ?? "en").Split('-')[0].ToUpperInvariant();

        public NumericData GetForCulture(string culture)
        {
            var norm = NormalizeCulture(culture);
            return _cache.GetOrAdd(norm, _ =>
            {
                var fileName = $"NumericData_{norm}.json";
                var filePath = Path.Combine(_resourcesPath, fileName);
                if (!File.Exists(filePath))
                {
                    Console.WriteLine($"Numeric data file not found for culture {norm}. Using fallback if available.");
                    return _fallback;
                }

                var json = File.ReadAllText(filePath);
                var data = JsonSerializer.Deserialize<NumericData>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                if (data == null)
                    throw new InvalidOperationException($"Failed to deserialize numeric data for culture {norm}.");
                return data;
            });
        }

        public void ClearCache() => _cache.Clear();
    }
}
