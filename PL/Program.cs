using AutoMapper;
using BLL.autoMapping;
using BLL.Interfaces;
using BLL.Repos;
using DAL.ContextDB;
using DAL.Entities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PL.eSettings;
using PL.Helper;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<TwoilioSetting>(builder.Configuration.GetSection("twilio"));
builder.Services.AddTransient<ISmsService, SmsService>();

builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IDepartmentRepo, DepartmentRepo>();
builder.Services.AddScoped<IEmployeeRepo, EmployeeRepo>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
//services.AddAutoMapper(typeof(EmployeeProfile));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, o =>
    {
        o.LogoutPath = new PathString("/Account/Login");
        o.AccessDeniedPath = new PathString("/Shared/Error");
    });
builder.Services.Configure<EmailSet>(builder.Configuration.GetSection("MailSettings"));
builder.Services.AddTransient<IEmailSettings, EmailSetting>();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(o =>
{
    o.Password.RequireDigit = true;
    o.Password.RequiredLength = 5;
    o.Password.RequireUppercase = true;
    //o.SignIn.RequireConfirmedAccount = true;
    o.Password.RequireLowercase = true;
    o.Password.RequireNonAlphanumeric = true;

}).AddEntityFrameworkStores<AppDbContexts>()
.AddTokenProvider<DataProtectorTokenProvider<ApplicationUser>>(TokenOptions.DefaultProvider);

builder.Services.AddDbContext<AppDbContexts>(o =>
{
    var DefaultConnection = builder.Configuration.GetConnectionString("DefaultConnection");

    o.UseSqlServer(DefaultConnection);
});
        

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Account}/{action=SignUp}/{id?}");
});
app.Run();