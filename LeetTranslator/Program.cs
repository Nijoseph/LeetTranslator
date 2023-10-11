using LeetTranslator.Services.Implementations;
using LeetTranslator.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add authentication services
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Home/Login";
        options.LogoutPath = "/Home/Logout";
    });

var connectionString = builder.Configuration.GetConnectionString("Translator2Leet");

// Register services
builder.Services.AddScoped<IUserDataServices>(provider => new UserDataServices(provider.GetRequiredService<ILogger<UserDataServices>>(), connectionString));
builder.Services.AddScoped<ITranslationDataServices, TranslationDataServices>(_ => new TranslationDataServices(connectionString));
builder.Services.AddScoped<ITranslationTypeDataServices, TranslationTypeDataServices>(_ => new TranslationTypeDataServices(connectionString));
builder.Services.AddScoped<IUserActivityDataServices, UserActivityDataServices>(_ => new UserActivityDataServices(connectionString));
builder.Services.AddScoped<IUserNotificationDataServices, UserNotificationDataServices>(_ => new UserNotificationDataServices(connectionString));
builder.Services.AddScoped<INotificationDataServices, NotificationDataServices>(_ => new NotificationDataServices(connectionString));
builder.Services.AddScoped<INotificationDataServices, NotificationDataServices>(_ => new NotificationDataServices(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if ( !app.Environment.IsDevelopment() )
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/");
app.Run();
