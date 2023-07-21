using BlazorChess.Client;
using BlazorChess.Client.Chess.Online;
using BlazorChess.Client.Services;
using ChessDotCore.Engine;
using ChessDotCore.Engine.Interfaces;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddSingleton<IChess, Chess>();
builder.Services.AddScoped<OnlineGameService>();
builder.Services.AddTransient<ClipboardService>();
builder.Services.AddTransient<AlertService>();
builder.Services.AddTransient<FullscreenService>();



await builder.Build().RunAsync();
