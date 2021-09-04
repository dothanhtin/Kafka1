using Confluent.Kafka;
using Core.IRepositories;
using Kafka1.Models;
using KafkaPubSub;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Serilog;
using Shared.Constants;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Services.EventHandler.OrderServices
{
    public class ProcessCreateOrdersService : BackgroundService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ConsumerConfig consumerConfig;
        private readonly ProducerConfig producerConfig;
        //private readonly ILogLibraryInterface _logger;
        public ProcessCreateOrdersService(ConsumerConfig consumerConfig, ProducerConfig producerConfig, IOrderRepository orderRepository)
        {
            this.producerConfig = producerConfig;
            this.consumerConfig = consumerConfig;
            _orderRepository = orderRepository;
        }

        [Obsolete]
#pragma warning disable CS0809 // Obsolete member overrides non-obsolete member
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
#pragma warning restore CS0809 // Obsolete member overrides non-obsolete member
        {
            Debug.WriteLine("OrderProcessing Service Started");
            //Test logstash
            //var logInput = _logger.WriteAsync("OrderProcessing Service Started", "ProcessOrdersService");
            Log.Information("OrderProcessing Service Started");
            OrderRequest order = new OrderRequest();
            while (!stoppingToken.IsCancellationRequested)
            {
                var consumerHelper = new ConsumerWrapper(consumerConfig, KafkaHelper.orderRequestTopic);
                string orderRequest = await consumerHelper.ReadMessage();

                if (!string.IsNullOrEmpty(orderRequest))
                {
                    //Deserilaize 
                    order = JsonConvert.DeserializeObject<OrderRequest>(orderRequest);

                    //TODO:: Process Order
                    Debug.WriteLine($"Info: OrderHandler => Processing the order for {order.orderId}");
                    //Write to database
                    await _orderRepository.createOrder(order, stoppingToken);
                }
            }
        }
    }
}
