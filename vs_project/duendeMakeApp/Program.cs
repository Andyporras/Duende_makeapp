using Microsoft.EntityFrameworkCore;
using duendeMakeApp.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using duendeMakeApp.DAO;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddDbContext<DuendeappContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("conexion")));
//que la conexion a la base de datos sea singleton
builder.Services.AddDbContext<DuendeappContext>(ServiceLifetime.Singleton);
//builder.Services.AddIdentity<Usuario, IdentityRole>()
//    .AddEntityFrameworkStores<DuendeappContext>()
//    .AddDefaultTokenProviders();


// Agregar el servicio de HttpClientFactory
builder.Services.AddHttpClient();

builder.Services.AddTransient<IEmailSender, EmailSenderDAO>();
builder.Services.AddScoped<Usuario>();

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
