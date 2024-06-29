using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SuperBazaar.Models;
using SuperBazaar.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
var Configuration = builder.Configuration;
builder.Services.AddDbContext<SuperBazarContext>(option => option.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddTransient<IAccounts, Accounts>();
builder.Services.AddTransient<IAutomatedHouseholdItemDispenser, AutomatedHouseholdItemDispenser>();
builder.Services.AddTransient<ISuperBazar, SuperBazar>();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
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
app.UseSession();
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=HomePage}/{action=HomePageView}/{id?}");

app.Run();
