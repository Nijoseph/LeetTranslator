using LeetTranslator.Services.Implementations;
using LeetTranslator.Services.Interfaces;
using LeetTranslator.Services;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add authentication services
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Home/Login";  // Redirect to login path if not authenticated
        options.LogoutPath = "/Home/Logout";  // Redirect to logout path when logging out
    });

builder.Services.AddScoped<IUserDataServices>(provider =>
{
    var logger = provider.GetRequiredService<ILogger<UserDataServices>>();
    var configuration = provider.GetRequiredService<IConfiguration>();
    var connectionString = configuration.GetConnectionString("Translator2Leet");
    return new UserDataServices(logger, connectionString);
});


builder.Services.AddScoped<ITranslationDataServices>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    var connectionString = configuration.GetConnectionString("Translator2Leet");
    return new TranslationDataServices(connectionString);
});

builder.Services.AddScoped<ITranslationTypeDataServices>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    var connectionString = configuration.GetConnectionString("Translator2Leet");
    return new TranslationTypeDataServices(connectionString);
});

builder.Services.AddScoped<IUserActivityDataServices>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    var connectionString = configuration.GetConnectionString("Translator2Leet");
    return new UserActivityDataServices(connectionString);
});

builder.Services.AddScoped<IUserNotificationDataServices>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    var connectionString = configuration.GetConnectionString("Translator2Leet");
    return new UserNotificationDataServices(connectionString);
});

builder.Services.AddScoped<INotificationDataServices>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    var connectionString = configuration.GetConnectionString("Translator2Leet");
    return new NotificationDataServices(connectionString);
});

builder.Services.AddScoped<IFunTranslationsService>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    var apiKey = configuration["FunTranslationsApiKey"];
    var apiUrl = configuration["FunTranslationsApiUrl"];
    return new FunTranslationsService(apiKey, apiUrl);
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Use Authentication and Authorization after UseRouting and before UseEndpoints
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
