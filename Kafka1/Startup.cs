using Confluent.Kafka;
using KafkaPubSub;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Shared.Connections;
using Shared.ConnectionConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.IRepositories;
using Repositories.OrderRepository;
using HostedServices = Microsoft.Extensions.Hosting;
using StackExchange.Redis.Extensions.Newtonsoft;
using StackExchange.Redis.Extensions.Core.Configuration;
using Services.EventHandler.OrderServices;
using Core.IServices.IOrderServices;
using Services.OrderServices;

namespace Kafka1
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            #region read comfiguration
            //Kafka
            ///Producer
            KafkaConfig.Instance.producerConfig = new ProducerConfig();
            Configuration.Bind("producer", KafkaConfig.Instance.producerConfig);
            ///Consumer
            KafkaConfig.Instance.consumerConfig = new ConsumerConfig();
            Configuration.Bind("consumer", KafkaConfig.Instance.consumerConfig);
            ///status of Kafka
            Configuration.Bind("status", KafkaConfig.Instance.status);
            //Database
            ///MongoDB
            Configuration.Bind("ConnectionStrings", mongoDBConfiguration);
            MongoDBConnection mongoDBConnection = new MongoDBConnection(mongoDBConfiguration);
            //Redis
            ///GetConfig
            RedisConfig.Instance.redisConfiguration = Configuration.GetSection("Redis").Get<RedisConfiguration>();
            #endregion
        }
        public MongoDBConfiguration mongoDBConfiguration = new MongoDBConfiguration();
        //public KafkaConfig kafkaConfig = new KafkaConfig();
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Kafka1", Version = "v1" });
            });
            //services.AddSingleton<HostedServices.IHostedService, ProcessOrdersService>();
            services.AddHostedService<ProcessCreateOrdersService>();
            //Add services Kafka
            services.AddSingleton(KafkaConfig.Instance.producerConfig);
            services.AddSingleton(KafkaConfig.Instance.consumerConfig);
            //Add services MongoDB
            services.AddSingleton(mongoDBConfiguration);
            //Add services Redis -> can inject to controller
            services.AddStackExchangeRedisExtensions<NewtonsoftSerializer>(RedisConfig.Instance.redisConfiguration);
            //Add services Repositories
            services.AddTransient<IOrderRepository, OrderRepository>();
            services.AddTransient<IProcessOrderServices, ProcessOrderServices>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            #region for dev
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //    app.UseSwagger();
            //    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Kafka1 v1"));
            //}
            #endregion

            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Kafka1 v1"));

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
