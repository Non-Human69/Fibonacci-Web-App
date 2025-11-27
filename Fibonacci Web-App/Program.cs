using Fibonacci_Web_App.Interfaces;
using Fibonacci_Web_App.Options;
using Fibonacci_Web_App.Providers;
using Fibonacci_Web_App.Repositories;
using Fibonacci_Web_App.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile(Path.Combine(builder.Environment.ContentRootPath, "Resources", "NumericData_EN.json"), optional: false, reloadOnChange: true);

builder.Services.AddRazorPages();

// Bind root configuration to NumericData (for start-up procces)
builder.Services.Configure<NumericData>(builder.Configuration);

// Register repository
builder.Services.AddSingleton<IFiboRepository, FiboRepository>();

// singletons -> once per project
builder.Services.AddSingleton<NumericDataProvider>();
builder.Services.AddSingleton<NumericCacheService>();

// scopeds -> once per http request
builder.Services.AddScoped<NumericWordsConverterService>();

// Localization configuration (query-string first)
var supportedCultures = new[] { new CultureInfo("en"), new CultureInfo("nl"), new CultureInfo("de") };

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new RequestCulture("en");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;

    options.RequestCultureProviders = new List<IRequestCultureProvider>
    {
        new QueryStringRequestCultureProvider(),
        new CookieRequestCultureProvider()
    };
});

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

var locOptions = app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value;
app.UseRequestLocalization(locOptions);

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapRazorPages();
app.Run();
