using Asp.Versioning;
using IDP.Application.Handler.Command.User;
using IDP.Domain.IRepository.Command;
using IDP.Domain.IRepository.Query;
using IDP.Infra.Data;
using IDP.Infra.Repository.Command;
using IDP.Infra.Repository.Query;
using MediatR;
using System.Reflection;
using IDP.Ioc;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

#region redisconfig
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetValue<string>("CachSetting:RedisUrl");
});

#endregion redisconfig

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1);
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ApiVersionReader = ApiVersionReader.Combine(
        new UrlSegmentApiVersionReader(),
        new HeaderApiVersionReader("X-Api-Version"));
})
.AddMvc() // This is needed for controllers
.AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'V";
    options.SubstituteApiVersionInUrl = true;
});

Auth.Extensions.AddJwt(builder.Services, builder.Configuration);

// Add MassTransit configuration

builder.Services.AddMassTransit(busConfig =>
{
    // Configure Entity Framework Outbox
    busConfig.AddEntityFrameworkOutbox<ShopCommandDbContext>(o =>
    {
        o.QueryDelay = TimeSpan.FromSeconds(30); // Check for messages every 30 seconds and send to RabitMq
        o.UseSqlServer(); // Use SQL Server as the database providerfto create Outbox table
        o.UseBusOutbox(); // Enable outbox for message publishing
    });

    // Set kebab-case endpoint naming
    busConfig.SetKebabCaseEndpointNameFormatter();

    // Configure RabbitMQ
    busConfig.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(new Uri(builder.Configuration.GetValue<string>("Rabbit:Host")), h =>
        {
            h.Username(builder.Configuration.GetValue<string>("Rabbit:Username"));
            h.Password(builder.Configuration.GetValue<string>("Rabbit:Password"));
        });

        // Retry policy for messages
        cfg.UseMessageRetry(r => r.Exponential(10, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(30),TimeSpan.FromSeconds(5)));

        // Configure endpoints for consumers
        cfg.ConfigureEndpoints(context);
    });
});

// Call the RegisterServices method
builder.Services.RegisterServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
