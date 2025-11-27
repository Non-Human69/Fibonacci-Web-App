using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace Fibonacci_Web_App.Pages
{
    public class SetLanguageModel : PageModel
    {
        public IActionResult OnPost(string culture, string? returnUrl)
        {
            if (string.IsNullOrWhiteSpace(culture))
            {
                return LocalRedirect(string.IsNullOrEmpty(returnUrl) ? "/" : returnUrl);
            }

            if (string.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
            {
                returnUrl = "/";
            }

            // Remove any existing "culture" query parameter from the returnUrl.
            var path = returnUrl;
            var queryDict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            var qIndex = returnUrl.IndexOf('?');
            if (qIndex >= 0)
            {
                path = returnUrl[..qIndex];
                var parsed = QueryHelpers.ParseQuery(returnUrl[qIndex..]);
                foreach (var kv in parsed)
                {
                    if (!string.Equals(kv.Key, "culture", StringComparison.OrdinalIgnoreCase))
                    {
                        // If there are multiple values, join with comma.
                        queryDict[kv.Key] = string.Join(",", kv.Value);
                    }
                }
            }

            // Rebuild URL without any existing culture, then add the selected one.
            var baseUrl = queryDict.Count > 0 ? QueryHelpers.AddQueryString(path, queryDict) : path;
            var separator = baseUrl.Contains('?') ? '&' : '?';
            var redirectUrl = $"{baseUrl}{separator}culture={culture}";

            // Persist the selected culture in a cookie so subsequent requests keep it.
            var cookieValue = CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture));
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                cookieValue,
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1), HttpOnly = false, IsEssential = true }
            );

            return LocalRedirect(redirectUrl);
        }
    }
}