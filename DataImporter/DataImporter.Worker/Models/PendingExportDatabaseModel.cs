using Autofac;
using DataImporter.Excel.BusinessObjects;
using DataImporter.Excel.Services;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataImporter.Worker.Models
{
    public class PendingExportDatabaseModel
    {
        private readonly IExportService _exportService;
        private IList<Export> PendingExports { get; set; }

        public PendingExportDatabaseModel()
        {
            _exportService = Worker.AutofacContainer.Resolve<IExportService>();
        }

        public PendingExportDatabaseModel(IExportService exportService)
        {
            _exportService = exportService;
        }

        public void LoadPendingExports()
        {
            PendingExports = _exportService.GetPendingExports();
        }

        public void SendPendingMail()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .Build();

            foreach (var export in PendingExports)
            {
                var message = new MimeMessage();

                var from = configuration.GetValue<string>("Email:From");
                var password = configuration.GetValue<string>("Email:password");
                var mailServer = configuration.GetValue<string>("Email:mailServer");
                var smtpPort = configuration.GetValue<int>("Email:smtpPort");
                var enableSsl = configuration.GetValue<bool>("Email:enableSsl");

                message.From.Add(MailboxAddress.Parse(from));
                message.To.Add(MailboxAddress.Parse(export.To));
                message.Subject = export.Subject;

                var builder = new BodyBuilder();

                builder.TextBody = export.TextBody;
                builder.Attachments.Add(export.FilePath);
                message.Body = builder.ToMessageBody();

                using (var client = new SmtpClient())
                {
                    client.Connect(mailServer, smtpPort, enableSsl);

                    client.Authenticate(from, password);

                    client.Send(message);
                    client.Disconnect(true);
                }

                ChangePandingStatus(export);
            }
        }

        private void ChangePandingStatus(Export export)
        {
            export.Status = "completed";
            _exportService.UpdateExport(export);
        }
    }
}
