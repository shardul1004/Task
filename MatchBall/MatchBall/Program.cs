using MatchBall.ApplicationContext;
using MatchBall.Filter;
using MatchBall.Services;
using MatchBall.Utilities;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var Configuration = builder.Configuration;
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddDbContext<TestDbContext>((options) => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IAccounts, Accounts>();
builder.Services.AddScoped<IBallMachine, BallMachine>();
builder.Services.AddSingleton<IHashing, Hashing>();
builder.Services.AddSingleton<IJWTUtil, JWTUtil>();
builder.Services.AddSingleton<JWTAuth>();
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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=HomePage}/{action=HomePageView}/{id?}");

app.Run();
