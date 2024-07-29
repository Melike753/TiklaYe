using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TiklaYe_CQRS.Data;
using TiklaYe_CQRS.CommandHandlers;
using MediatR;
using TiklaYe_CQRS.QueryHandlers;
using TiklaYe_CQRS.Services;
using System.Reflection;
using TiklaYe_CQRS.Models;
using TiklaYe_CQRS.Services;
using TiklaYe_CQRS.Queries;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

// Session servislerini ekleme.
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

var dbconnection = builder.Configuration.GetConnectionString("dbConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(dbconnection));

// Add MediatR and register handlers
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

// Add command and query handlers
builder.Services.AddScoped<AddProductCommandHandler>();
builder.Services.AddScoped<AddToCartCommandHandler>();
builder.Services.AddScoped<ApproveBusinessCommandHandler>();
builder.Services.AddScoped<ClearCartCommandHandler>();
builder.Services.AddScoped<CompleteOrderCommandHandler>();
builder.Services.AddScoped<CreateCategoryCommandHandler>();
builder.Services.AddScoped<CreateFeedbackCommandHandler>();
builder.Services.AddScoped<DeleteCategoryCommandHandler>();
builder.Services.AddScoped<DeleteProductCommandHandler>();
builder.Services.AddScoped<DeleteUserHandler>();
builder.Services.AddScoped<DownloadInvoiceHandler>();
builder.Services.AddScoped<EditProductCommandHandler>();
builder.Services.AddScoped<ICartService, TiklaYe_CQRS.Models.CartService>();
builder.Services.AddScoped<LoginBusinessOwnerCommandHandler>();
builder.Services.AddScoped<LoginCommandHandler>();
builder.Services.AddScoped<LogoutCommandHandler>();
builder.Services.AddScoped<ProductCreateCommandHandler>();
builder.Services.AddScoped<ProductDeleteCommandHandler>();
builder.Services.AddScoped<ProductUpdateCommandHandler>();
builder.Services.AddScoped<RegisterBusinessOwnerCommandHandler>();
builder.Services.AddScoped<RegisterUserCommandHandler>();
builder.Services.AddScoped<RemoveFromCartCommandHandler>();
builder.Services.AddScoped<UpdateCategoryCommandHandler>();
builder.Services.AddScoped<UpdateProfileHandler>();
builder.Services.AddScoped<UpdateStatusHandler>();
builder.Services.AddScoped<AdminGetAllProductsQueryHandler>();
builder.Services.AddScoped<AdminGetProductByIdQueryHandler>();
builder.Services.AddScoped<GetAllProductsQueryHandler>();
builder.Services.AddScoped<GetAllUsersHandler>();
builder.Services.AddScoped<GetCategoriesQueryHandler>();
builder.Services.AddScoped<GetCategoryByIdQueryHandler>();
builder.Services.AddScoped<GetGroupedPurchasesHandler>();
builder.Services.AddScoped<GetPendingBusinessRequestsQueryHandler>();
builder.Services.AddScoped<GetFeedbacksQueryHandler>();
builder.Services.AddScoped<GetProductByIdQueryHandler>();
builder.Services.AddScoped<GetSalesReportQueryHandler>();
builder.Services.AddScoped<GetUserByEmailQueryHandler>();
builder.Services.AddScoped<GetUserByUsernameHandler>();
builder.Services.AddScoped<GetUserPurchaseHistoryHandler>();
builder.Services.AddScoped<GetUserPurchasesHandler>();
builder.Services.AddScoped<UserExistsQueryHandler>();
builder.Services.AddScoped<ICartQueryService, CartQueryService>();
builder.Services.AddScoped<ProductQueryService>();

// Add authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.AccessDeniedPath = "/Home/AccessDenied";
        options.LoginPath = "/Business/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
    });

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.Run();
