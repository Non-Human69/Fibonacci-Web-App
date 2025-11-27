using Fibonacci_Web_App.Interfaces;
using System.Numerics;
using System.Text;
using System.Collections.Concurrent;
using System.Linq;
using Fibonacci_Web_App.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using System.IO;
using System;
using Fibonacci_Web_App.Options;
using Microsoft.Extensions.Options;


namespace Fibonacci_Web_App.Repositories
{
    public class NumericWordsConverterRepository : INumericWordsConverterRepository
    {
        public NumericData _numericData { get; set; }
        public List<(ScaleItem Item, BigInteger Value)> _scaleValues { get; set; }
        public ConcurrentDictionary<BigInteger, string> _cache { get; set; } = new();
        public NumericWordsConverterRepository(IOptions<NumericData> options)
        {
            _numericData = options.Value;
            _scaleValues = GetScaleValues();
        }

        private NumericData LoadNumericDataFromJson(string culture = "en")
        {
            NumericData numericData = new NumericData();

            // default
            var jsonPath = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "NumericData_EN.json");

            try
            {
                switch(culture)
                {
                    case "nl":
                        jsonPath = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "NumericData_NL.json");
                        break;
                    default:
                        jsonPath = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "NumericData_EN.json");
                        break;
                }
            }
            catch
            {
                // fallback to default (English) if anything goes wrong reading/parsing cookie
                jsonPath = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "NumericData_EN.json");
            }
 
            if (!File.Exists(jsonPath))
            {
                throw new FileNotFoundException($"The file at path {jsonPath} was not found.");
            }

            var jsonData = File.ReadAllText(jsonPath);
            var options = new System.Text.Json.JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            numericData = System.Text.Json.JsonSerializer.Deserialize<NumericData>(jsonData, options);
             
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

        public void ResetOrChangeLanguage(string culture = "en")
        {
            _cache.Clear();
            _numericData = LoadNumericDataFromJson(culture);
        }
    }
}
