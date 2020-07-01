using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using shop_giay_server.data;

namespace shop_giay_server.Crons
{
    public class DailyCron : CronJobService
    {
        private readonly ILogger<DailyCron> _logger;
        private readonly DataContext _context;
        public DailyCron(IScheduleConfig<DailyCron> config, ILogger<DailyCron> logger, DataContext context)
            : base(config.CronExpression, config.TimeZoneInfo)
        {
            _context = context;
            _logger = logger;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Daily cron job starts.");
            return base.StartAsync(cancellationToken);
        }

        public override Task DoWork(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{DateTime.Now:hh:mm:ss} Daily cron job is working.");

            var now = DateTime.Now;
            var sales = _context.Sales.ToList();

            // update sales which were expired
            foreach (var sale in sales)
            {
                if (sale.ExpiredDate <= now)
                {
                    sale.Status = 0;

                    _context.Sales.Update(sale);
                }
            }

            _context.SaveChangesAsync();

            return Task.CompletedTask;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Daily cron job is stopping.");
            return base.StopAsync(cancellationToken);
        }
    }
}