using IDP.Application.Handler.Command.User;
using IDP.Domain.IRepository.Command;
using IDP.Domain.IRepository.Query;
using IDP.Infra.Data;
using IDP.Infra.Repository.Command;
using IDP.Infra.Repository.Query;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IDP.Ioc
{
    public static class DependencyContainer
    {
        public static void RegisterServices(this IServiceCollection services)
        {

            services.AddMediatR(typeof(UserHandler).GetTypeInfo().Assembly);
            services.AddScoped<IOtpRedisRepository, OtpRedisRepository>();
            services.AddTransient<IUserQueryRepository, UserQueryRepository>();
            services.AddTransient<IUserCommandRepository, UserCommandRepository>();

            services.AddDbContext<ShopCommandDbContext>();
            services.AddDbContext<ShopQueryDbContext>();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


            // Register CAP
            #region Add Cap library
            //builder.Services.AddCap(options =>
            //{
            //    options.UseEntityFramework<ShopCommandDbContext>(); // to create table outbox and inbox
            //    options.UseDashboard(path => path.PathMatch = "/cap-dashboard");
            //    options.UseRabbitMQ(options =>
            //    {
            //        options.ConnectionFactoryOptions = options =>
            //        {
            //            options.Ssl.Enabled = false; // because of development environment
            //            options.HostName = "localhost";
            //            options.UserName = "guest"; // rabbit user
            //            options.Password = "guest"; // rabbit pass
            //            options.Port = 5672;
            //        };
            //    });
            //    options.FailedRetryCount = 20; // if rabbit not responsive how many time I sent request again
            //    options.FailedRetryInterval = 5;  // how many second I send request again to Rabbit
            //});

            #endregion




            //// Add MassTransit configuration
            //services.AddMassTransit(busConfig =>
            //{
            //    // Configure Entity Framework Outbox
            //    busConfig.AddEntityFrameworkOutbox<ShopCommandDbContext>(o =>
            //    {
            //        o.QueryDelay = TimeSpan.FromSeconds(30); // Check for messages every 30 seconds
            //        o.UseSqlServer(); // Use SQL Server as the database provider
            //        o.UseBusOutbox(); // Enable outbox for message publishing
            //    });

            //    // Set kebab-case endpoint naming
            //    busConfig.SetKebabCaseEndpointNameFormatter();

            //    // Configure RabbitMQ
            //    busConfig.UsingRabbitMq((context, cfg) =>
            //    {
            //        cfg.Host(new Uri(builder.Configuration.GetValue<string>("Rabbit:Host")), h =>
            //        {
            //            h.Username(builder.Configuration.GetValue<string>("Rabbit:Username"));
            //            h.Password(builder.Configuration.GetValue<string>("Rabbit:Password"));
            //        });

            //        // Retry policy for messages
            //        cfg.UseMessageRetry(r => r.Exponential(10, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(30)));

            //        // Configure endpoints for consumers
            //        cfg.ConfigureEndpoints(context);
            //    });
            //});

        }
    }
}
