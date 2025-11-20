using Fibonacci_Web_App.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Numerics;

namespace Fibonacci_Web_App.Pages
{
    public class NumberToTextModel : PageModel
    {
        public INumericWordsConverterRepository NumericWordsConverterRepository { get; }

        [BindProperty]
        public BigInteger inputNumber { get; set; }

        public NumberToTextModel(INumericWordsConverterRepository numericWordsConverterRepository)
        {
            this.NumericWordsConverterRepository = numericWordsConverterRepository;
        }
    }
}