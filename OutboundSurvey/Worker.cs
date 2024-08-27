namespace OutboundSurvey
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            Task.Run(() =>
            {
                Application.StartApplication();
            });
            await base.StartAsync(cancellationToken);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await base.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(60*60*1000, stoppingToken);
                Log.Logger.Info("--- -- {Windows Service HeartBeat} -- ----");
            }
        }

        public override void Dispose()
        {
        }
    }
}