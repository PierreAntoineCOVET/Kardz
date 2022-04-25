using MassTransit;

namespace TimedEventWorker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IBusControl _busControl;

        public Worker(ILogger<Worker> logger, IBusControl busControl)
        {
            _logger = logger;
            _busControl = busControl;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return _busControl.StartAsync(stoppingToken);
        }
    }
}