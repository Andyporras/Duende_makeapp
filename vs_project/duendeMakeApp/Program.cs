using Microsoft.EntityFrameworkCore;
using duendeMakeApp.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();


builder.Services.AddDbContext<DuendeappContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("conexion")));

// Agregar el servicio de HttpClientFactory
builder.Services.AddHttpClient();

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
