using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Threading.Tasks;

namespace ICS.Web.User.EmailAdapters
{
    public class EmailSender : IEmailSender
    {

        public EmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor)
        {
            Options = optionsAccessor.Value;
        }

        public AuthMessageSenderOptions Options { get; } //set only via Secret Manager

        public Task SendEmailAsync(string email, string subject, string message)
        {
            return Execute(subject, message, email);
        }

        public Task Execute(string subject, string message, string email)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress(Options.SendName, Options.SenderUser));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = "If you are registered on the ICS.Web." + message + Environment.NewLine + Environment.NewLine + "This email is automatically generated. Please do not reply to it."
            };

            using (var client = new SmtpClient())
            {
                client.Connect("smtp.yandex.ru", 25, false);
                client.AuthenticateAsync(Options.SenderUser, Options.SendPassword).Wait();
                client.SendAsync(emailMessage).Wait();
                client.DisconnectAsync(true).Wait();
                return Task.CompletedTask;
            }
        }
    }
}
