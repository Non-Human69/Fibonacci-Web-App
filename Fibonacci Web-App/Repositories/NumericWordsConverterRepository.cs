using Fibonacci_Web_App.Interfaces;
using Fibonacci_Web_App.Models;
using System.Numerics;
using System.Text;
using System.Collections.Concurrent;
using System.Linq;
using Fibonacci_Web_App.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using System.IO;
using System;

namespace Fibonacci_Web_App.Repositories
{
    public class NumericWordsConverterRepository : INumericWordsConverterRepository
    {
        public NumericData _numericData;
        public List<(ScaleItem Item, BigInteger Value)> _scaleValues;
        public readonly ConcurrentDictionary<BigInteger, string> _cache = new();
        private readonly IHttpContextAccessor _httpContextAccessor;

        public NumericWordsConverterRepository(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _numericData = LoadNumericDataFromJson();
            _scaleValues = GetScaleValues();
        }
        
        private NumericData LoadNumericDataFromJson()
        {
            NumericData numericData = new NumericData();

            // default
            var jsonPath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "NumericData_EN.json");

            // get culture->language from cookies
            try
            {
                var httpContext = _httpContextAccessor?.HttpContext;
                if (httpContext != null)
                {
                    var cookieName = CookieRequestCultureProvider.DefaultCookieName;
                    if (httpContext.Request.Cookies.TryGetValue(cookieName, out var cultureCookie) && !string.IsNullOrEmpty(cultureCookie))
                    {
                        var providerResult = CookieRequestCultureProvider.ParseCookieValue(cultureCookie);
                        var culture = providerResult?.Cultures?.FirstOrDefault().Value;

                        if (!string.IsNullOrEmpty(culture))
                        {
                            // support "nl" and variants like "nl-NL"
                            if (culture.StartsWith("nl", StringComparison.OrdinalIgnoreCase))
                            {
                                jsonPath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "NumericData_NL.json");
                            }
                            else
                            {
                                jsonPath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "NumericData_EN.json");
                            }
                        }
                    }
                }
            }
            catch
            {
                // fallback to default (English) if anything goes wrong reading/parsing cookie
                jsonPath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "NumericData_EN.json");
            }

            if (File.Exists(jsonPath))
            {
                var jsonData = File.ReadAllText(jsonPath);
                var options = new System.Text.Json.JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                numericData = System.Text.Json.JsonSerializer.Deserialize<NumericData>(jsonData, options);
            }
            else
            {
                throw new FileNotFoundException($"The file at path {jsonPath} was not found.");
            }

            return numericData;

        }
        private List<(ScaleItem Item, BigInteger Value)> GetScaleValues()
        {
            List<(ScaleItem Item, BigInteger Value)> scaleValues = new List<(ScaleItem Item, BigInteger Value)>();
            scaleValues = _numericData.unnamed
                .Select(s => (Item: s, Value: BigInteger.Pow(10, s.Power)))
                .OrderByDescending(t => t.Item.Power)
                .ToList();
            scaleValues.AddRange(_numericData.Rest
                .Select(s => (Item: s, Value: BigInteger.Pow(10, s.Power)))
                .OrderByDescending(t => t.Item.Power)
                .ToList());

            Console.WriteLine("Scale values initialized:");
            Console.WriteLine($"Biggest scale value: {scaleValues.Last().Item.Name} with {scaleValues.Last().Value}");
            return scaleValues;
        }

        public string ConvertToWords(BigInteger i)
        {
            NumericWordsConverterService service = new NumericWordsConverterService(_numericData, _scaleValues, _cache);
            return service.ConvertToWords(i);
        }
        public void ResetOrChangeLanguage()
        {
            _cache.Clear();
            _numericData = LoadNumericDataFromJson();
        }
    }
}
