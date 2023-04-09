namespace Alumni_Back.Services
{
    public class PointBackgroundService : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while(!stoppingToken.IsCancellationRequested)
            {

                await Task.Delay(TimeSpan.FromDays(1),stoppingToken);
            }
        }
    }
}
