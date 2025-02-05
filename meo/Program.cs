
using meo.DataAccess.Data;
using meo.DataAccess.Repository;
using meo.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using meo.Utility;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using meo.Models;
using meo.DbInitializer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
//---------------------------------------------------------------------------------------
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer
(builder.Configuration.GetConnectionString("DefaultConnection")));

//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<ApplicationDbContext>();





builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
builder.Services.AddRazorPages();

builder.Services.ConfigureApplicationCookie(options => {

    options.LoginPath = $"/Identity/Account/Login";
    options.LogoutPath = $"/Identity/Account/Logout";
    options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
    
});

// add facebook authentication

builder.Services.AddAuthentication().AddFacebook(options =>
{
    options.AppId = "572726168935390";
    options.AppSecret = "ef269c0c3efbd79bfae81afdcba26300";
});

// add google authentication
builder.Services.AddAuthentication().AddGoogle(options =>
{
    options.ClientId = "1090292520927-n8hcmp4v0f4u1peg91j9mdadadjdl72u.apps.googleusercontent.com";
    options.ClientSecret = "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";

    });
//-----------------------------
//-- Add session services to the container.----------------------------------------------
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
//---------------------------------------------------------------------------------------
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddScoped<IDbInitializer, Dbinitializer>();

//----------------------------------------------------------------------------------------
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for Categoryion scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
//---
app.UseSession();
//---
//---
SeedDatabase();
//---
app.MapRazorPages();
app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();

void SeedDatabase()
{
    using (var scope = app.Services.CreateScope())
    {
        var DbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
        DbInitializer.Initialize();
    }
}