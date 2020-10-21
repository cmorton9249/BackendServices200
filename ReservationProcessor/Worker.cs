using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ReservationProcessor.Utils;

namespace ReservationProcessor
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly ConsumerConfig _config;

        public Worker(ILogger<Worker> logger, ConsumerConfig config)
        {
            _logger = logger;
            _config = config;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumerHelper = new ConsumerWrapper("libraryreservations", _config);
            _logger.LogInformation("The Service is running and waiting for reservations");
            while (!stoppingToken.IsCancellationRequested)
            {
                var order = consumerHelper.ReadMessage<ReservationMessage>();
                _logger.LogInformation($"Got a reservation for {order.For} for the items {order.Items}");
            }
            
        }
    }


    public class ReservationMessage
    {
        public int Id { get; set; }
        public string For { get; set; }
        public string Items { get; set; }
        public string Status { get; set; }
    }




}
