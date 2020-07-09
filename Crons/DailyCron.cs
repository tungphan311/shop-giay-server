using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using shop_giay_server.data;
using Microsoft.Extensions.DependencyInjection;

namespace shop_giay_server.Crons
{
    public class DailyCron : CronJobService
    {
        private readonly ILogger<DailyCron> _logger;
        public IServiceScopeFactory _serviceScopeFactory;
        public DailyCron(IScheduleConfig<DailyCron> config, ILogger<DailyCron> logger, IServiceScopeFactory serviceScopeFactory)
            : base(config.CronExpression, config.TimeZoneInfo)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Daily cron job starts.");
            return base.StartAsync(cancellationToken);
        }

        public async override Task DoWork(CancellationToken cancellationToken)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                DataContext _context = (DataContext)scope.ServiceProvider.GetRequiredService(typeof(DataContext));
                _logger.LogInformation($"{DateTime.Now:hh:mm:ss} Daily cron job is working.");

                var now = DateTime.Now;
                var sales = _context.Sales.ToList();

                // update sales which were expired
                foreach (var sale in sales)
                {
                    int result = DateTime.Compare(now, sale.ExpiredDate);
                    Console.WriteLine(result);
                    if (result >= 0)
                    {
                        sale.Status = 0;

                        _context.Sales.Update(sale);
                    }
                }

                await _context.SaveChangesAsync();
            }

            await Task.CompletedTask;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Daily cron job is stopping.");
            return base.StopAsync(cancellationToken);
        }
    }
}