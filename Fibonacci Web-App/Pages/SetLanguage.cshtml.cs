using Fibonacci_Web_App.Interfaces;
using Fibonacci_Web_App.Repositories;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Fibonacci_Web_App.Pages
{
    public class SetLanguageModel : PageModel
    {
        public INumericWordsConverterRepository NumericWordsConverterRepository { get; }
        public IActionResult OnPost(string culture, string? returnUrl)
        {
            if (string.IsNullOrWhiteSpace(culture))
            {
                return LocalRedirect(string.IsNullOrEmpty(returnUrl) ? "/" : returnUrl);
            }

            var cookieValue = CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture));
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                cookieValue,
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1), IsEssential = true }
            );

            if (string.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
            {
                return LocalRedirect("/");
            }

            NumericWordsConverterRepository.ResetOrChangeLanguage();
            return LocalRedirect(returnUrl);
        }
        public SetLanguageModel(INumericWordsConverterRepository numericWordsConverterRepository)
        {
            NumericWordsConverterRepository = numericWordsConverterRepository;
        }
    }
}