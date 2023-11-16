using Microsoft.EntityFrameworkCore;
using duendeMakeApp.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity.UI.Services;
using duendeMakeApp.DAO;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();


builder.Services.AddDbContext<DuendeappContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("conexion"))
        );
//que la coneccion sea singleton
builder.Services.AddDbContext<DuendeappContext>(ServiceLifetime.Singleton);

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Maquillajes/";
        //options.LogoutPath = "/";
        //options.AccessDeniedPath = "/AccessDenied";
        //options.Cookie.Name = "Duendeapp";
        //options.Cookie.HttpOnly = true;
        //options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
        //options.SlidingExpiration = true;
    });

// Agregar el servicio de HttpClientFactory
builder.Services.AddHttpClient();

builder.Services.AddTransient<IEmailSender, EmailSenderDAO>();
builder.Services.AddScoped<Usuario>();
builder.Services.AddHttpContextAccessor();

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
app.UseAuthentication();;

app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Maquillajes}/{action=Index}");

app.UseEndpoints(endpoints =>
{
    endpoints.MapRazorPages();
});

//app.MapRazorPages();

app.Run();
