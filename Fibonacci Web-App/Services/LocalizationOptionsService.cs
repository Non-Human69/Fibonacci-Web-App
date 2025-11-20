using System.Globalization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;

namespace Fibonacci_Web_App.Services
{
    public class LocalizationOptionsService : IConfigureOptions<RequestLocalizationOptions>
    {
        public void Configure(RequestLocalizationOptions options)
        {
            var supportedCultures = new[] { new CultureInfo("en"), new CultureInfo("nl") };
            options.DefaultRequestCulture = new RequestCulture(supportedCultures[0]);
            options.SupportedCultures = supportedCultures;
            options.SupportedUICultures = supportedCultures;
            options.RequestCultureProviders.Insert(0, new QueryStringRequestCultureProvider());
            options.RequestCultureProviders.Add(new CookieRequestCultureProvider());
        }
    }
} 