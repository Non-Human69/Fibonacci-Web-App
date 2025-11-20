namespace Fibonacci_Web_App.Models
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
}
