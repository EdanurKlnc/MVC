using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using PhoneBookBusinessLayer.EmailSenderBusiness;
using PhoneBookBusinessLayer.ImplementatinOfManagers;
using PhoneBookBusinessLayer.InterfacesOfManagers;
using PhoneBookDataLayer;
using PhoneBookDataLayer.ImplementationOfRepo;
using PhoneBookDataLayer.ImplementationsOfRepo;
using PhoneBookDataLayer.InterfaceOfRepo;
using PhoneBookEntityLayer.Mappings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<MyContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Local"));
});

//CookiesAuthentication ayar�
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();
builder.Services.AddAutoMapper(x =>
{
    x.AddExpressionMapping();
    x.AddProfile(typeof(Maps)); //Kimin kime d�n��eceini maps class ta tan�mlad�k. Yapt���m�z tan�mlamay� ayarlara ekledik.
});
// Add services to the container.
builder.Services.AddControllersWithViews();

//interfacelerin i�lerini ger�ekle�tirecek classlar� burada ya�am d�ng�lerini (inject) etmeliyiz.
builder.Services.AddScoped<IMemberManager, MemberManager>();
builder.Services.AddScoped<IMemberRepository, MemberRepository>();
builder.Services.AddScoped <IEmailSender , EmailSender>();

builder.Services.AddScoped <IPhoneTypeRepository , PhoneTypeRepository>();
builder.Services.AddScoped<IPhoneTypeManager, PhoneTypeManager>();

builder.Services.AddScoped<IMemberPhoneRepository, MemberPhoneRepository>();
builder.Services.AddScoped<IMemberPhoneManager, MemberPhoneManager>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles(); //

app.UseRouting(); //browserdki url i�in hoe/index gidebilmesi i�in

app.UseAuthentication(); //login ve logout i�lemleri i�in
app.UseAuthorization(); //Yetkilendirme i�in

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Register}/{id?}");

app.Run();
