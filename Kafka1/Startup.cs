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
using Services;

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
            kafkaConfig.producerConfig = new ProducerConfig();
            Configuration.Bind("producer", kafkaConfig.producerConfig);
            ///Consumer
            kafkaConfig.consumerConfig = new ConsumerConfig();
            Configuration.Bind("consumer", kafkaConfig.consumerConfig);
            //Database
            ///MongoDB
            Configuration.Bind("ConnectionStrings", mongoDBConfiguration);
            MongoDBConnection mongoDBConnection = new MongoDBConnection(mongoDBConfiguration);
            #endregion
        }
        public MongoDBConfiguration mongoDBConfiguration = new MongoDBConfiguration();
        public KafkaConfig kafkaConfig = new KafkaConfig();
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Kafka1", Version = "v1" });
            });
            services.AddSingleton<HostedServices.IHostedService, ProcessOrdersService>();
            //Add services Kafka
            services.AddSingleton<ProducerConfig>(kafkaConfig.producerConfig);
            services.AddSingleton<ConsumerConfig>(kafkaConfig.consumerConfig);
            //Add services MongoDB
            services.AddSingleton<MongoDBConfiguration>(mongoDBConfiguration);
            //Add services Repositories
            services.AddTransient<IOrderRepository, OrderRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Kafka1 v1"));
            }

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
