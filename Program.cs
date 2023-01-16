using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using TaskManagement;

var builder = WebApplication.CreateBuilder(args);

var politicaUsuariosAutenticados = new AuthorizationPolicyBuilder()
    .RequireAuthenticatedUser()
    .Build();

// Add services to the container.
builder.Services.AddControllersWithViews(opciones =>
{
    opciones.Filters.Add(new AuthorizeFilter(politicaUsuariosAutenticados));
});

builder.Services.AddDbContext<ApplicationDbContext>(options => 
options.UseSqlServer("name=DefaultConnection"));

builder.Services.AddAuthentication();

builder.Services.AddIdentity<IdentityUser,IdentityRole>(opciones =>
{
    opciones.SignIn.RequireConfirmedAccount = false;  //si esta en true el user tiene que confirmar su cuenta via email, phone etc
}).AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders(); // con todo esto tenemos aplicado Identity en nuestra aplicaci�n 

builder.Services.PostConfigure<CookieAuthenticationOptions>(IdentityConstants.ApplicationScheme,
  opciones =>
{
    opciones.LoginPath = "/usuarios/login";
    opciones.AccessDeniedPath= "/usuarios/login"; // configuramos nuestras access routes 
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

app.UseRouting();

//necesitamos un middleware antes de UseAuthorization()

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
