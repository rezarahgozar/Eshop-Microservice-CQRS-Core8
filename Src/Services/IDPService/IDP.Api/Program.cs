using Asp.Versioning;
using IDP.Application.Handler.Command.User;
using IDP.Domain.IRepository.Command;
using IDP.Domain.IRepository.Query;
using IDP.Infra.Data;
using IDP.Infra.Repository.Command;
using IDP.Infra.Repository.Query;
using MediatR;
using System.Reflection;


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

builder.Services.AddMediatR(typeof(UserHandler).GetTypeInfo().Assembly);
builder.Services.AddScoped<IOtpRedisRepository,OtpRedisRepository>();
builder.Services.AddTransient<IUserQueryRepository, UserQueryRepository>();
builder.Services.AddTransient<IUserCommandRepository, UserCommandRepository>();

builder.Services.AddDbContext<ShopCommandDbContext>();
builder.Services.AddDbContext<ShopQueryDbContext>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

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

// Register CAP
#region Add Cap library
builder.Services.AddCap(options =>
{
    options.UseEntityFramework<ShopCommandDbContext>(); // to create table outbox and inbox
    options.UseDashboard(path => path.PathMatch = "/cap-dashboard");
    options.UseRabbitMQ(options =>
    {
        options.ConnectionFactoryOptions = options =>
        {
            options.Ssl.Enabled = false; // because of development environment
            options.HostName = "localhost";
            options.UserName = "guest"; // rabbit user
            options.Password = "guest"; // rabbit pass
            options.Port = 5672;
        };
    });
    options.FailedRetryCount = 20; // if rabbit not responsive how many time I sent request again
    options.FailedRetryInterval = 5;  // how many second I send request again to Rabbit
});

#endregion



Auth.Extensions.AddJwt(builder.Services, builder.Configuration);

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
