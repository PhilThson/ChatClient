using Microsoft.AspNetCore.ResponseCompression;
using BlazorServer.Data;
using BlazorServer.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder =>
        {
            //wg dokumentacji jest to wymagane zeby podlaczyc zewnetrznych klientow
            builder.WithOrigins("http://localhost:5000")
                .AllowAnyHeader()
                .WithMethods("GET", "POST")
                .AllowCredentials();
        });
});

builder.Services.AddResponseCompression(opts =>
{
    // upewnienie sie ze nasz serwer moze przetwarzac pakiety z naglowkiem
    // octet-stream
    // i kompresowanie tych polaczen
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        new[] { "application/octet-stream" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
//udana próba podłączenia zewnętrznego klienta:
//app.UseCors(c =>
//{
//    c.AllowAnyHeader();
//    c.SetIsOriginAllowed(s => s == "http://localhost:5000");
//    c.AllowCredentials();
//});

//poniżej wykorzystanie domyślnego klienta
app.UseCors();

//app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

// blazor hub jest po czesci po stronie serwera a po czesci po stronie klienta
app.MapBlazorHub();

app.MapHub<ChatHub>("/chathub");
app.MapHub<CounterHub>("/counterhub");

app.MapFallbackToPage("/_Host");

app.Run();

