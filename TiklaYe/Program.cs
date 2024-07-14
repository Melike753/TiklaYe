using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TiklaYe.Data;
using TiklaYe.Models; // User modelini eklediðinizden emin olun
using Microsoft.AspNetCore.Identity; // Gerekli using direktifini ekleyin


var builder = WebApplication.CreateBuilder(args);
// Configuration
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

var dbconnection = builder.Configuration.GetConnectionString("dbConnection"); // appsetting.json

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(dbconnection));


// Authorization
builder.Services.AddAuthorization();

// Controllers and Views
builder.Services.AddControllersWithViews();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.Run();