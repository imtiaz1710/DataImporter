using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataImporter.Membership.Services
{
    public class MessageService : IEmailSender
    {
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("membershipSetting.json", true, true)
                .Build();

            var to = /*configuration.GetValue<string>("Email:From");*/ email;
            var from = "imtiazmehedidemo@gmail.com";
            var password = /*configuration.GetValue<string>("Email:password");*/ "imtiazdemo";
            var mailServer = /*configuration.GetValue<string>("Email:mailServer");*/ "smtp.gmail.com";
            var smtpPort = /*configuration.GetValue<int>("Email:smtpPort");*/ 587;
            var enableSsl = /*configuration.GetValue<bool>("Email:enableSsl");*/ false;

            var mimeMessage = new MimeMessage();

            mimeMessage.From.Add(MailboxAddress.Parse(from));
            mimeMessage.To.Add(MailboxAddress.Parse(to));
            mimeMessage.Subject = subject;

            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = htmlMessage;
            mimeMessage.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                client.Connect(mailServer, smtpPort, enableSsl);
                client.Authenticate(from, password);

                await client.SendAsync(mimeMessage);
                await client.DisconnectAsync(true);
            }
        }
    }
}
