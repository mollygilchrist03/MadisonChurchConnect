using MadisonChurchConnect.Services.BusinessLogic;
using MadisonChurchConnect.Services.DataAccess;
using MadisonChurchConnect.Services.Interfaces;
using MadisonChurchConnect.Services.YouTube;
using Microsoft.AspNetCore.DataProtection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews();

// Register dependency injection services
builder.Services.AddScoped<INoteDAO, NoteDAO>();
builder.Services.AddScoped<NoteLogic>();
builder.Services.Configure<YouTubeOptions>(builder.Configuration.GetSection(YouTubeOptions.SectionName));
builder.Services.AddHttpClient<IYouTubeService, YouTubeService>();
// register user services
builder.Services.AddScoped<IUserDAO,UserDAO>();
builder.Services.AddScoped<UserLogic>();

// register cookie authentication
builder.Services.AddAuthentication(Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login/Login";
    });

builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(@"C:\home\ASP.NET\DataProtection-Keys"))
    .SetApplicationName("MadisonChurchConnect");

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Sermons}/{action=Index}/{id?}");

app.Run();
