using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using MVCPhonebook;
using MVCPhonebook.Data;
using MVCPhonebook.Data.Contract;
using MVCPhonebook.Data.Implementation;
using MVCPhonebook.Services.Contract;
using MVCPhonebook.Services.Implementation;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Database connection
builder.Services.AddDbContextPool<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("mydb"));
});

// Configure JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["AppSettings:Token"])),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
        };
    });

// Configure services (Business layer)
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IContactService, ContactService>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<ITokenService, TokenService>();

// Configure services (Data layer)
builder.Services.AddScoped<IAppDbContext>(provider => (IAppDbContext)provider.GetService(typeof(AppDbContext)));
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IContactRepository, ContactRepository>();

var app = builder.Build();

// Inject IWebHostEnvironment
var env = app.Services.GetRequiredService<IWebHostEnvironment>();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

// Add the JwtTokenMiddleware
app.UseMiddleware<JwtTokenMiddleware>();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(env.ContentRootPath, "Uploads")),
    RequestPath = "/Uploads"
});

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Phonebook}/{action=Index}/{id?}");

app.Run();
