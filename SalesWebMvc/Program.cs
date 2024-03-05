using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using MySqlConnector;
using SalesWebMvc.Data;
using SalesWebMvc.Models.Services;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using System.Configuration;
using Microsoft.Extensions.Localization;


var builder = WebApplication.CreateBuilder(args);

var connection = builder.Configuration.GetConnectionString(name:"SalesWebMvcContext");

builder.Services.AddDbContext<SalesWebMvcContext>(options =>
    options.UseMySql(connection, ServerVersion.AutoDetect(connection)));

builder.Services.AddScoped<SellerService>();
builder.Services.AddScoped<DepartmentService>();
builder.Services.AddScoped<SalesRecordService>();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

using var scope = app.Services.CreateScope();

using SalesWebMvcContext context = scope.ServiceProvider.GetRequiredService<SalesWebMvcContext>();

SeedingService seedingService = new SeedingService(context);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
    var enUS = new CultureInfo("en-US");
    var localizationOptions = new RequestLocalizationOptions 
    { 
        DefaultRequestCulture = new RequestCulture("en-US"),
        SupportedCultures = new List<CultureInfo> { enUS },
        SupportedUICultures = new List<CultureInfo> { enUS }
    };

    app.UseRequestLocalization(localizationOptions);

    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseRouting();

    app.UseAuthorization();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    seedingService.Seed();

    app.Run();