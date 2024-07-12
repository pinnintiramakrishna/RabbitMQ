using MassTransit;
using RabbitMQPublisher.Services.PublishMessageService;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Host
    .UseSerilog((context, services, configuration) =>
        configuration
            .ReadFrom.Configuration(context.Configuration)
            .ReadFrom.Services(services)
            .Enrich.FromLogContext());
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IPublisher, MassTransitPublisher>();

//
var host = builder.Configuration.GetSection("RabbitMqConfiguration:Host").Get<string>();
var port = builder.Configuration.GetSection("RabbitMqConfiguration:Port").Get<string>();
var virtualHost = builder.Configuration.GetSection("RabbitMqConfiguration:VirtualHost").Get<string>();
var userId = builder.Configuration.GetSection("RabbitMqConfiguration:UserId").Get<string>();
var password = builder.Configuration.GetSection("RabbitMqConfiguration:Password").Get<string>();

if (string.IsNullOrEmpty(host) || string.IsNullOrEmpty(port) || string.IsNullOrEmpty(virtualHost)
    || string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(password)) throw new Exception("Missing configuration for RabbitMq");

builder.Services.AddMassTransit(x =>
{
    x.AddDelayedMessageScheduler();
    x.SetKebabCaseEndpointNameFormatter();
    //var entryAssembly = Assembly.GetEntryAssembly();
    //x.AddConsumers(entryAssembly);
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(host, port, virtualHost, h =>
        {
            h.Username(userId);
            h.Password(password);
        });

        cfg.UseDelayedMessageScheduler();
        cfg.ConfigureEndpoints(context);
    });
});
//

var app = builder.Build();

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
