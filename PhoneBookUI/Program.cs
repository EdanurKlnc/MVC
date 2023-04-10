using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PhoneBookDataLayer;
using PhoneBookEntityLayer.Entities.Mappings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<MyContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Local"));
});
builder.Services.AddAutoMapper(x =>
{
    x.AddProfile(typeof(Maps)); //Kimin kime dönüþeceini maps class ta tanýmladýk. Yaptýðýmýz tanýmlamayý ayarlara ekledik.
});
// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
