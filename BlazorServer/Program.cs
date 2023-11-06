﻿using Microsoft.AspNetCore.ResponseCompression;
using BlazorServer.Hubs;
using BlazorServer.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddHttpClient();
builder.Services.AddScoped<TokenService>();

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

app.UseCors();

//app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();

app.MapHub<ChatHub>("/chathub");

app.MapFallbackToPage("/_Host");

app.Run();

