using System.Numerics;

namespace Fibonacci_Web_App.Options
{
    public class NumericData
    {
        public List<string> ZeroTo99 { get; set; } = new();
        public string Hundred { get; set; } = string.Empty;
        public string Thousand { get; set; } = string.Empty;
        public List<ScaleItem> Rest { get; set; } = new();
        public List<ScaleItem> unnamed { get; set; } = new();
    }

    public class ScaleItem
    {
        public string Name { get; set; } = string.Empty;
        public int Power { get; set; }
    }

    public class ScaleValue
    {
        public ScaleItem Item { get; set; }
        public BigInteger Value { get; set; }
    }
}
