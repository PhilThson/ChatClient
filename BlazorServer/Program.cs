using Microsoft.AspNetCore.ResponseCompression;
using BlazorServer.Data;
using BlazorServer.Helpers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddHttpClient(ChatConstants.HttpAuthClient, config =>
{
    config.BaseAddress = new Uri(ChatConstants.AuthBaseUrl);

    config.Timeout = TimeSpan.FromSeconds(30);
    config.DefaultRequestHeaders.Clear();
    config.DefaultRequestHeaders.Add("Accept", "application/json");
});
builder.Services.AddHttpClient(ChatConstants.HttpChatClient, config =>
{
    config.BaseAddress = new Uri(ChatConstants.ChatBaseUrl + "api/");

    config.Timeout = TimeSpan.FromSeconds(30);
    config.DefaultRequestHeaders.Clear();
    config.DefaultRequestHeaders.Add("Accept", "application/json");
});
//.AddHttpMessageHandler<HttpClientMiddleware>();

builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<ChatService>();
builder.Services.AddTransient<HttpClientMiddleware>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder =>
        {
            //needed to connect clients
            builder.WithOrigins("http://localhost:5000")
                .AllowAnyHeader()
                .WithMethods("GET", "POST")
                .AllowCredentials();
        });
});

builder.Services.AddResponseCompression(opts =>
{
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

app.UseCors();

//app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();

app.MapFallbackToPage("/_Host");

app.Run();

