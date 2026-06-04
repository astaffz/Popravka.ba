using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Popravka.ba.Data;
using PopravkaBa.Application.Services;
using PopravkaBa.Application.Services.Implementation;
using PopravkaBa.Application.Services.Interface;
using PopravkaBa.Domain.Interfaces;
using PopravkaBa.Domain.Interfaces.Repositories;
using PopravkaBa.Domain.Models;
using PopravkaBa.Infrastructure.Adapters;
using PopravkaBa.Infrastructure.Repositories;
using PopravkaBa.Infrastructure.Seeders;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.User.RequireUniqueEmail = true;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
    options.Lockout.MaxFailedAccessAttempts = 10;
    options.Lockout.AllowedForNewUsers = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();
builder.Services.AddControllersWithViews();

// Dodaj rate limiter
builder.Services.AddRateLimiter(options =>
{
    // Login rate limiter
    options.AddPolicy("auth", httpContext =>
    RateLimitPartition.GetFixedWindowLimiter(
        // Particijski rate limiter, vrši rate limit po IP adresi umjesto globalno za sve korisnike
        partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
        factory: _ => new FixedWindowRateLimiterOptions
        {
            PermitLimit = 10,
            Window = TimeSpan.FromMinutes(1),
            QueueLimit = 0
        }));
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
});

// TODO 1HIGHPRIORITY: Da li implementirati na engleskom aplikaciju?
// TODO Registriraj sve dependency injectione po dodavanju, implementirati zakomentarisane
// Dependency Injection za interfejse (Možda treba ubaciti u metodu)   
builder.Services.AddScoped<IKategorijaService, KategorijaService>();
builder.Services.AddScoped<IKategorijaRepository, KategorijaRepository>();

builder.Services.AddScoped<IMjestoService, MjestoService>();
builder.Services.AddScoped<IMjestoRepository,MjestoRepository>();


builder.Services.AddScoped<IOglasRepository, OglasRepository>();
builder.Services.AddScoped<IOglasService, OglasService>();

builder.Services.AddScoped<IOglasMajstoraService, OglasMajstoraService>();
builder.Services.AddScoped<IOglasMajstoraRepository, OglasMajstoraRepository>();

builder.Services.AddScoped<IOglasRadnoMjestoService, OglasRadnoMjestoService>();
builder.Services.AddScoped<IOglasRadnoMjestoRepository, OglasRadnoMjestoRepository>();
builder.Services.AddScoped<IUvjetOglasaRepository, UvjetOglasaRepository>();
builder.Services.AddScoped<IUvjetOglasaService, UvjetOglasaService>();

builder.Services.AddScoped<IOglasUslugeService, OglasUslugeService>();
builder.Services.AddScoped<IOglasUslugeRepository, OglasUslugeRepository>();

builder.Services.AddScoped<IOglasMajstoraFacade, OglasMajstoraFacade>();
builder.Services.AddScoped<IOglasRadnoMjestoFacade, OglasRadnoMjestoFacade>();
builder.Services.AddScoped<IOglasUslugeFacade, OglasUslugeFacade>();


builder.Services.AddScoped<IPonudaUslugeService, PonudaUslugeService>();
// builder.Services.AddScoped<IPrijavaOglasService, PrijavaOglasService>();
builder.Services.AddScoped<IRecenzijaService, RecenzijaService>();


builder.Services.AddScoped<IEmailSender, SmtpEmailAdapter>();
builder.Services.AddScoped<DbSeeder>();

var app = builder.Build();

// Lokalni blok koda za kreiranje uloga i popunavanju baze pri pokretanju aplikacije, NE STAVITI IZNAD LINIJE builder.Build()!
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
    var seeder =  scope.ServiceProvider.GetRequiredService<DbSeeder>();
    await seeder.SeedAsync();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
    app.UseExceptionHandler("/Home/Error");
    app.UseStatusCodePagesWithReExecute("/greska/{0}"); // Ne bi trebalo biti ovdje kad bi se puštao u production
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseStatusCodePagesWithReExecute("/greska/{0}");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();
app.UseRateLimiter();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();
