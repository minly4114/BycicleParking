using ICS.Core.Engine.IProviders;
using ICS.Core.Host.Engine.IProviders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ICS.Core.Host.BackgroundTask
{
    public class BackgroundHostedSessionControl : BackgroundService
    {
        private readonly ILog4netProvider _logger;

        public BackgroundHostedSessionControl(IServiceProvider services,
            ILog4netProvider logger)
        {
            Services = services;
            _logger = logger;
        }

        public IServiceProvider Services { get; }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.Info(typeof(BackgroundHostedSessionControl).ToString(),
                "Session control hosted Service running.");

            await DoWork(stoppingToken);
        }

        private async Task DoWork(CancellationToken stoppingToken)
        {
            _logger.Info(typeof(BackgroundHostedSessionControl).ToString(),
                "Session control hosted Service is working.");

            using (var scope = Services.CreateScope())
            {
                var sessionsProvider =
                    scope.ServiceProvider
                        .GetRequiredService<ISessionsProvider>();
                while (!stoppingToken.IsCancellationRequested)
                {
                    _logger.Info(typeof(BackgroundHostedSessionControl).ToString(), "Check reservation place execute");
                    TimeSpan time = new TimeSpan(12, 0, 0);
                    var sessions = sessionsProvider.GetSessionReservation().Where(x => DateTime.UtcNow - x.StartParking > time).ToList();
                    _logger.Info(typeof(BackgroundHostedSessionControl).ToString(), $"Check reservation place execute{Environment.NewLine}Overdue sessions", sessions.ConvertAll(x => x.ToOutcome()));
                    List<Task> tasks = new List<Task>();
                    sessions.ForEach(x => tasks.Add(sessionsProvider.CancelReservation(x)));
                    Task.WaitAll(tasks.ToArray());
                    _logger.Info(typeof(BackgroundHostedSessionControl).ToString(), "Check reservation place success execute");
                    await Task.Delay(3600000, stoppingToken);
                }
            }
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.Info(typeof(BackgroundHostedSessionControl).ToString(),
                "Session control hosted Service is stopping.");

            await Task.CompletedTask;
        }
    }
}
