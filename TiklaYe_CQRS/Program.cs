using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using TiklaYe_CQRS.Data;
using TiklaYe_CQRS.CommandHandlers;
using MediatR;
using TiklaYe_CQRS.QueryHandlers;
using TiklaYe_CQRS.Services;

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

builder.Services.AddMediatR(typeof(Program));
builder.Services.AddMediatR(typeof(CompleteOrderCommandHandler).Assembly);
builder.Services.AddSession();
builder.Services.AddScoped<ActivateBusinessCommandHandler>();
builder.Services.AddScoped<AddProductCommandHandler>();
builder.Services.AddScoped<AddToCartCommandHandler>();
builder.Services.AddScoped<ApproveBusinessCommandHandler>();
builder.Services.AddScoped<ClearCartCommandHandler>();
builder.Services.AddScoped<CompleteOrderCommandHandler>();
builder.Services.AddScoped<CreateCategoryCommandHandler>();
builder.Services.AddScoped<CreateFeedbackCommandHandler>();
builder.Services.AddScoped<DeactivateBusinessCommandHandler>();
builder.Services.AddScoped<DeleteCategoryCommandHandler>();
builder.Services.AddScoped<DeleteProductCommandHandler>();
builder.Services.AddScoped<DeleteUserCommandHandler>();
builder.Services.AddScoped<DownloadInvoiceCommandHandler>();
builder.Services.AddScoped<EditProductCommandHandler>();
builder.Services.AddScoped<LoginCommandHandler>();
builder.Services.AddScoped<ProductCreateCommandHandler>();
builder.Services.AddScoped<ProductDeleteCommandHandler>();
builder.Services.AddScoped<ProductUpdateCommandHandler>();
builder.Services.AddScoped<RegisterCommandHandler>();
builder.Services.AddScoped<RegisterUserCommandHandler>();
builder.Services.AddScoped<RemoveFromCartCommandHandler>();
builder.Services.AddScoped<UpdateBusinessProfileCommandHandler>();
builder.Services.AddScoped<UpdateCategoryCommandHandler>();
builder.Services.AddScoped<UpdateStatusCommandHandler>();
builder.Services.AddScoped<UpdateUserProfileCommandHandler>();
builder.Services.AddScoped<AdminGetAllProductsQueryHandler>();
builder.Services.AddScoped<AdminGetProductByIdQueryHandler>();
builder.Services.AddScoped<GetAllProductsQueryHandler>();
builder.Services.AddScoped<GetAllUsersQueryHandler>();
builder.Services.AddScoped<GetBusinessProfileQueryHandler>();
builder.Services.AddScoped<GetCartItemsQueryHandler>();
builder.Services.AddScoped<GetCategoriesQueryHandler>();
builder.Services.AddScoped<GetCategoryByIdQueryHandler>();
builder.Services.AddScoped<GetFeedbacksQueryHandler>();
builder.Services.AddScoped<GetGroupedPurchasesQueryHandler>();
builder.Services.AddScoped<GetPaymentQueryHandler>();
builder.Services.AddScoped<GetPendingBusinessRequestsQueryHandler>();
builder.Services.AddScoped<GetProductByIdQueryHandler>();
builder.Services.AddScoped<GetPurchaseHistoryQueryHandler>();
builder.Services.AddScoped<GetRestaurantRevenueQueryHandler>();
builder.Services.AddScoped<GetSalesReportQueryHandler>();
builder.Services.AddScoped<GetUserByEmailQueryHandler>();
builder.Services.AddScoped<GetUserByUsernameQueryHandler>();
builder.Services.AddScoped<GetUserProfileQueryHandler>();
builder.Services.AddScoped<GetUserPurchasesQueryHandler>();
builder.Services.AddScoped<LoginQueryHandler>();
builder.Services.AddScoped<UserExistsQueryHandler>();
builder.Services.AddScoped<ICartQueryService, CartQueryService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<ProductQueryService>();
builder.Services.AddTransient<EmailService>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.AccessDeniedPath = "/Home/Index";
        options.LoginPath = "/Account/Login";
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