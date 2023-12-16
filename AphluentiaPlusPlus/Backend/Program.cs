using Backend.Configs;
using Backend.Providers;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Register RedisCacheConfigSection first
builder.Services.Configure<GatewayConfigSection>(builder.Configuration.GetSection("GatewayConfigSection"));

builder.Services.AddSingleton<IGatewayProvider, GatewayProvider>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
app.UseHttpsRedirection();
app.UseSession();
app.UseAuthorization();
app.MapControllers();
app.Run();

