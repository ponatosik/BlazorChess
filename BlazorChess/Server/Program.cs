using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using BlazorChess.Server.Hubs;
using BlazorChess.Server.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddSignalR();
builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
       new[] { "application/octet-stream" });
});

if (builder.Environment.IsDevelopment())
{
	builder.Services.AddDbContext<ChessDbContext>(options =>
		options.UseInMemoryDatabase("InMemoryDatabase"));
}
else
{
	var mySqlVersion = new MySqlServerVersion(new Version(5, 7, 9));
	builder.Services.AddDbContext<ChessDbContext>(options =>
		options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), mySqlVersion));
}

var app = builder.Build();

app.UseResponseCompression();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseWebAssemblyDebugging();
}
else
{
	app.UseExceptionHandler("/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();


app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.MapHub<ChessHub>("/chesshub");

app.Run();
