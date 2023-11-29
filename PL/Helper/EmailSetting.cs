using DAL.Entities;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using PL.eSettings;

namespace PL.Helper
{
    public class EmailSetting : IEmailSettings
    {
        private readonly EmailSet options;

        public EmailSetting(IOptions<EmailSet> _options) 
        {
            options = _options.Value;
        }
        public void SendEmail(Email email)
        {
            var mail = new MimeMessage
            {
                Sender = MailboxAddress.Parse(options.Email),
                Subject = email.Subject
            };

            mail.To.Add(MailboxAddress.Parse(email.to));


            var builder = new BodyBuilder();

            builder.HtmlBody = email.body;
			mail.Body = builder.ToMessageBody();

			mail.From.Add(new MailboxAddress(options.DisplayName,options.Email));

            using var smtp = new SmtpClient();
            smtp.Connect(options.Host,options.Port, SecureSocketOptions.StartTls);

            smtp.Authenticate(options.Email,options.Password);
            smtp.Send(mail);

            smtp.Disconnect(true);
        }
    }
}
