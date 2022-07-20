using EAuction.Data.MessageBroker;
using EAuction.Data.Repository;
using EAuction.Models.API;
using EAuction.Models.Seller;
using EAuction.Processor.Helpers;
using EAuction.Processor.Interface;
using EAuction.Processor.Processors;
using EAuction.Processor.Services;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.IO;
using System.Text;

namespace EAuctionBuyer
{
    public class Startup
    {
        public static AppSettings AppSettings { get; set; }

        public static ServiceInformation ServiceInformation { get; set; }

        public static RabbitMQInformation RabbitMQInformation { get; set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            var appSettingsJson = File.ReadAllText("appsettings.json");

            AppSettings = JsonConvert.DeserializeObject<AppSettings>(appSettingsJson);

            ServiceInformation = JsonConvert.DeserializeObject<ServiceInformation>(appSettingsJson);

            RabbitMQInformation = AppSettings.RabbitMQInformation;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var description = new StringBuilder();
            description.AppendLine($"Environment : {ServiceInformation.Environment} {Environment.NewLine}");
            description.AppendLine($"Build : {ServiceInformation.Build} {Environment.NewLine}");
            description.AppendLine($"Release : {ServiceInformation.Release} {Environment.NewLine}");

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = ServiceInformation.ServiceName,
                    Version = "v1",
                    Description = description.ToString()
                });
            });

            services.AddApiVersioning(x =>
            {
                x.DefaultApiVersion = new ApiVersion(1, 0);
                x.AssumeDefaultVersionWhenUnspecified = true;
                x.ReportApiVersions = true;
            });

            services.AddVersionedApiExplorer(setup =>
            {
                setup.GroupNameFormat = "'v'VVV";
                setup.SubstituteApiVersionInUrl = true;
            });

            services.AddControllers();

            services.AddSingleton(service =>
            {
                return new ConnectionFactory()
                {
                    HostName = RabbitMQInformation.HostName,
                    UserName = RabbitMQInformation.UserName,
                    Password = RabbitMQInformation.Password,
                    Port = RabbitMQInformation.Port,
                    VirtualHost = RabbitMQInformation.VirtualHost,
                };
            });

            services.AddScoped<IRabbitMqProducer, RabbitMqProducer>()
                .AddScoped<IProcessData, ProcessData>()
                .AddScoped<IServiceBusConsumer, ServiceBusConsumer>()
                .AddScoped<IServiceBusPublisher, ServiceBusPublisher>();

            services.Configure<MongoDBSettings>(Configuration.GetSection("MongoDB"))
            .Configure<ServiceBusSettings>(Configuration.GetSection("ServiceBus"));

            services
                .AddSingleton<AuctionDBContext>()
                .AddTransient<IValidator<User>, UserValidator>()
                .AddTransient<IProductRepository, ProductRepository>()
                .AddTransient<IUserRepository, UserRepository>()
                .AddTransient<IProductToBuyerRepository, ProductToBuyerRepository>()
                .AddTransient<IProductService, ProductService>()
                .AddTransient<IUserService, UserService>()
                .AddTransient<IBuyerProcessor, BuyerProcessor>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            app.UseCors(builder =>
                builder
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
              );

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(options =>
            {
                //c.SwaggerEndpoint("/swagger/v1/swagger.json", ServiceInformation.ServiceName);
                options.RoutePrefix = "swagger";
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                }
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
