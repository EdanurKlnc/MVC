using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PhoneBookBusinessLayer.EmailSenderBusiness;
using PhoneBookBusinessLayer.ImplementatinOfManagers;
using PhoneBookBusinessLayer.InterfacesOfManagers;
using PhoneBookDataLayer;
using PhoneBookDataLayer.ImplementationOfRepo;
using PhoneBookDataLayer.InterfaceOfRepo;
using PhoneBookEntityLayer.Mappings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<MyContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Local"));
});
builder.Services.AddAutoMapper(x =>
{
    x.AddProfile(typeof(Maps)); //Kimin kime dönüşeceini maps class ta tanımladık. Yaptığımız tanımlamayı ayarlara ekledik.
});
// Add services to the container.
builder.Services.AddControllersWithViews();

//interfacelerin işlerini gerçekleştirecek classları burada yaşam döngülerini (inject) etmeliyiz.
builder.Services.AddScoped<IMemberManager, MemberManager>();
builder.Services.AddScoped<IMemberRepository, MemberRepository>();
builder.Services.AddScoped <IEmailSender , EmailSender>();

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
