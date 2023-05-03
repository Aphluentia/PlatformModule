using Bridge.BackgroundService.Interfaces;
using Bridge.BackgroundService.Monitors;
using Bridge.BackgroundService.Threads;
using Bridge.ConfigurationSections;
using Bridge.Dtos.Enum;
using Bridge.Dtos.Status;
using Bridge.Providers;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<KafkaConfigSection>(builder.Configuration.GetSection("KafkaConfigSection"));
builder.Services.Configure<ServerSocketConfigSection>(builder.Configuration.GetSection("ServerSocketConfigSection"));

// Add your shared resource here
builder.Services.AddSingleton<MKafka>();
builder.Services.AddSingleton<TKafkaConsumer>();


builder.Services.AddSingleton<ConnectionManagerProvider>();
builder.Services.AddSingleton<TMessageHandler>();


builder.Services.AddSingleton<KafkaStatus>();
builder.Services.AddSingleton<ConnectionProviderStatus>();



// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();


var messageHandler = app.Services.GetService<TMessageHandler>();
messageHandler?.Start();

// Register the factory method for message handler with a Port argument

var kafkaConsumer = app.Services.GetService<TKafkaConsumer>();
kafkaConsumer?.Start();



    
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
