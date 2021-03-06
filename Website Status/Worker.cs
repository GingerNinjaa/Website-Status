using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Website_Status
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private HttpClient client;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            client = new HttpClient();

            return base.StartAsync(cancellationToken);
        }
        public override Task StopAsync(CancellationToken cancellationToken)
        {
            client.Dispose();
            return base.StopAsync(cancellationToken);
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                //https
                var result = await client.GetAsync("https://www.dlewandowski.pl");

                if (result.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Website is up. Statuc code {StatusCode}", result.StatusCode);
                }
                else
                {
                    _logger.LogError("WEBSITE IS DOWN {StatusCode}", result.StatusCode);
                }

                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}
