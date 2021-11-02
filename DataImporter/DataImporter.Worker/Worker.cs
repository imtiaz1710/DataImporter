using Autofac;
using DataImporter.Worker.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DataImporter.Worker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        public static ILifetimeScope AutofacContainer { get; private set; }

        public Worker(ILogger<Worker> logger, ILifetimeScope autofacContainer)
        {
            _logger = logger;
            AutofacContainer = autofacContainer;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
           
            while (!stoppingToken.IsCancellationRequested)
            {
                var importDatabaseModel = new PendingImportDatabaseModel();
                importDatabaseModel.LoadPendingImports();
                importDatabaseModel.UploadPendingFileToDb();
                importDatabaseModel.ChangePandingStatus();
                importDatabaseModel.DeletePendingFiles();

                var exportDatabaseModel = new PendingExportDatabaseModel();
                exportDatabaseModel.LoadPendingExports();
                exportDatabaseModel.SendPendingMail();

                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(10000, stoppingToken);
            }
        }
    }
}