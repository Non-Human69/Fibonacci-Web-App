using Fibonacci_Web_App.Interfaces;
using Fibonacci_Web_App.Repositories;
using Fibonacci_Web_App.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Globalization;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

// Using LocalizationOptionsService
builder.Services.ConfigureOptions<LocalizationOptionsService>();

builder.Services.AddSingleton<IFiboRepository, FiboRepository>();
builder.Services.AddSingleton<INumericWordsConverterRepository, NumericWordsConverterRepository>();

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Apply the configured localization options
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
