using Amazon;
using Amazon.S3;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Popravka.ba.Data;
using PopravkaBa.Application.Services;
using PopravkaBa.Application.Services.Implementation;
using PopravkaBa.Application.Services.Interface;
using PopravkaBa.Application.Strategies.Implementation;
using PopravkaBa.Application.Strategies.Interface;
using PopravkaBa.Domain.Interfaces;
using PopravkaBa.Domain.Interfaces.Repositories;
using PopravkaBa.Domain.Models;
using PopravkaBa.Domain.Shared;
using PopravkaBa.Infrastructure.Adapters;
using PopravkaBa.Infrastructure.Adapters.Options;
using PopravkaBa.Infrastructure.BackgroundServices;
using PopravkaBa.Infrastructure.Repositories;
using PopravkaBa.Infrastructure.Seeders;
using System.Globalization;
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

    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequiredUniqueChars = 0;

})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddErrorDescriber<BosanskiIdentityErrorDescriber>()
.AddPasswordValidator<CaseInsensitivePasswordValidator<ApplicationUser>>()
.AddDefaultTokenProviders();
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/login";
    options.LogoutPath = "/logout";
    options.AccessDeniedPath = "/login";
});
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

// TODO Registriraj sve dependency injectione po dodavanju, implementirati zakomentarisane
// Dependency Injection za interfejse (Možda treba ubaciti u metodu)   
builder.Services.AddScoped<IKategorijaService, KategorijaService>();
builder.Services.AddScoped<IKategorijaRepository, KategorijaRepository>();

builder.Services.AddScoped<IMjestoService, MjestoService>();
builder.Services.AddScoped<IMjestoRepository, MjestoRepository>();

builder.Services.AddScoped<IMjesecnaStatistikaRepository, MjesecnaStatistikaRepository>();

builder.Services.AddScoped<IOglasRepository, OglasRepository>();
builder.Services.AddScoped<IOglasService, OglasService>();

builder.Services.AddScoped<IOglasMajstoraService, OglasMajstoraService>();
builder.Services.AddScoped<IOglasMajstoraRepository, OglasMajstoraRepository>();

builder.Services.AddScoped<IOglasRadnoMjestoService, OglasRadnoMjestoService>();
builder.Services.AddScoped<IOglasRadnoMjestoRepository, OglasRadnoMjestoRepository>();
builder.Services.AddScoped<IUvjetOglasaRepository, UvjetOglasaRepository>();
builder.Services.AddScoped<IUvjetOglasaService, UvjetOglasaService>();
builder.Services.AddScoped<IVozackeDozvoleRepository, VozackeDozvoleRepository>();
builder.Services.AddScoped<IVozackeDozvoleService, VozackeDozvoleService>();

builder.Services.AddScoped<IOglasUslugeService, OglasUslugeService>();
builder.Services.AddScoped<IOglasUslugeRepository, OglasUslugeRepository>();
builder.Services.AddScoped<IIzvrsilacUslugeRepository, IzvrsilacUslugeRepository>();

builder.Services.AddScoped<IOglasMajstoraFacade, OglasMajstoraFacade>();
builder.Services.AddScoped<IOglasRadnoMjestoFacade, OglasRadnoMjestoFacade>();
builder.Services.AddScoped<IOglasUslugeFacade, OglasUslugeFacade>();

builder.Services.AddScoped<IPonudaUslugeRepository, PonudaUslugeRepository>();
builder.Services.AddScoped<IPonudaUslugeService, PonudaUslugeService>();

builder.Services.AddScoped<IPrijavaRadnoMjestoRepository, PrijavaRadnoMjestoRepository>();
builder.Services.AddScoped<IPrijavaOglasService, PrijavaOglasService>();
builder.Services.AddScoped<IRecenzijaService, RecenzijaService>();
builder.Services.AddScoped<IRecenzijaRepository, RecenzijaRepository>();

builder.Services.AddScoped<IPretragaStrategy, KlijentStrategy>();
builder.Services.AddScoped<IPretragaStrategy, MajstorStrategy>();
builder.Services.AddScoped<IPretragaStrategy, FirmaStrategy>();
builder.Services.AddScoped<IPretragaStrategy, AdministratorStrategy>();

builder.Services.AddScoped<IPretragaService, PretragaService>();

builder.Services.AddSingleton<IStatistikaStrategy, SortStatistikaKategorija>();
builder.Services.AddSingleton<IStatistikaStrategy, SortStatistikaMajstor>();
builder.Services.AddSingleton<IStatistikaStrategy, SortStatistikaMjesto>();
builder.Services.AddSingleton<IStatistikaStrategy, SortStatistikaOcjena>();
builder.Services.AddSingleton<IStatistikaStrategy, SortStatistikaPoslovi>();
builder.Services.AddSingleton<IStatistikaSortResolver, StatistikaSortResolver>();
builder.Services.AddScoped<IStatistikaService, StatistikaService>();
builder.Services.AddScoped<IMjesecnaStatistikaRepository, MjesecnaStatistikaRepository>();

builder.Services.AddScoped<IEmailSender, BrevoEmailAdapter>();
builder.Services.Configure<BrevoEmailOptions>(builder.Configuration.GetSection("Brevo"));

builder.Services.Configure<R2Options>(builder.Configuration.GetSection("R2"));

builder.Services.AddSingleton<IAmazonS3>(sp =>
{
    var o = sp.GetRequiredService<IOptions<R2Options>>().Value;
    return new AmazonS3Client(o.AccessKey, o.SecretKey, new AmazonS3Config
    {
        ServiceURL = $"https://{o.AccountID}.r2.cloudflarestorage.com",
        AuthenticationRegion = "auto",
        ForcePathStyle = true,
   
    });
});
builder.Services.AddSingleton<IFileStorage, S3FileStorageAdapter>();

builder.Services.AddScoped<IVerifikacijaEmailaService, VerifikacijaEmailaService>();
builder.Services.AddScoped<IVerifikacijskiTokenRepository, VerifikacijskiTokenRepository>();

builder.Services.AddScoped<IVerifikacijaFirmeService, VerifikacijaFirmeService>();
builder.Services.AddScoped<IVerifikacijaFirmeRepository, VerifikacijaFirmeRepository>();

builder.Services.AddHostedService<VerifikacijskiTokenCleanupJob>();
builder.Services.AddHostedService<MjesecnaStatistikaJob>();
// builder.Services.AddHostedService<NeaktivniOglasiCleanupJob>();
builder.Services.AddScoped<DbSeeder>();



builder.Services.Configure<SecurityStampValidatorOptions>(options =>
{
    // Ostavljeno za određivanje trajanja sesije, ako je korisnik otišao/korisniku obrisan nalog
    options.ValidationInterval = TimeSpan.FromMinutes(15);
});



var app = builder.Build();

// Lokalni blok koda za kreiranje uloga i popunavanju baze pri pokretanju aplikacije, NE STAVITI IZNAD LINIJE builder.Build()!
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
    var seeder = scope.ServiceProvider.GetRequiredService<DbSeeder>();
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

// Lokalizacija: bazirana na InvariantCulture (decimalni separator ostaje "."),
// ali s dd/MM/yyyy formatom datuma za prikaz i model binding formi.
var bhKultura = (CultureInfo)CultureInfo.InvariantCulture.Clone();
bhKultura.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
bhKultura.DateTimeFormat.DateSeparator = "/";
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture(bhKultura),
    SupportedCultures = new[] { bhKultura },
    SupportedUICultures = new[] { bhKultura }
});

app.UseRouting();
app.UseRateLimiter();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "profil",
    pattern: "Profil/{username}",
    defaults: new { controller = "Profil", action = "Index" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();
