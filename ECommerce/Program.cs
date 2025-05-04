using ECommerce.Context;
using ECommerce.Repository;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddHttpClient();
builder.Services.AddScoped<OrderRepository>();
builder.Services.AddScoped<ProductRepository>();
builder.Services.AddScoped<OrderDetailRepository>();
builder.Services.AddScoped<OrderNoSeqRepository>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<ProductService>();


builder.Services.AddDbContext<ECommerceDbContext>(options =>
    options.UseSqlite("Data Source=ecommerce.db"));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Replace UseEndpoints with top-level route registrations
app.MapRazorPages();
app.MapControllers();
app.MapControllerRoute(
   name: "default",
   pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
