using Fibonacci_Web_App.Options;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.IO;
using System.Text.Json;

namespace Fibonacci_Web_App.Providers
{
    public class NumericDataProvider
    {
        private readonly ConcurrentDictionary<string, NumericData> _cache = new();
        private readonly string _resourcesPath;
        private readonly NumericData? _fallback;

        public NumericDataProvider(IHostEnvironment env)
        {
            _resourcesPath = Path.Combine(env.ContentRootPath, "Resources");

            var fallbackFile = Path.Combine(_resourcesPath, "NumericData_EN.json");
            if (File.Exists(fallbackFile))
            {
                try
                {
                    var json = File.ReadAllText(fallbackFile);
                    _fallback = JsonSerializer.Deserialize<NumericData>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
                catch (System.Exception ex)
                {
                    throw new InvalidOperationException("Failed to load fallback numeric data for EN culture.", ex);
                }
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
                    if (_fallback != null) return _fallback;
                    throw new InvalidOperationException($"Numeric data file not found for culture {norm} and no fallback available.");
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
