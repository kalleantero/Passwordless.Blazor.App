using Passwordless.Blazor.App.Extensions;
using Passwordless.Blazor.App.Interfaces;
using Passwordless.Blazor.App.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddAuthentication("Cookies").AddCookie();

builder.Services.AddHttpClient<IPasswordlessService, PasswordlessService>(client =>
{
    client.BaseAddress = new Uri("https://v4.passwordless.dev/");
    client.DefaultRequestHeaders.Add("ApiSecret", builder.Configuration["Passwordless:ApiSecret"]);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAuthentication();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.MapBlazorHub();

app.MapFallbackToPage("/_Host");

var passwordlessLoginService = app.Services.GetRequiredService<IPasswordlessService>();

app.MapLoginEndpoints(passwordlessLoginService);

app.Run();
