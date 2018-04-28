using System.Linq;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
using Workflow.Abstractions.Models;
using Workflow.Abstractions.Services;
using Workflow.Emailing.Configuration;

namespace Workflow.Emailing.Services
{
    public class EmailService : IEmailService
    {
        private readonly IEmailingConfiguration _emailingConfiguration;


        public EmailService(IEmailingConfiguration emailingConfiguration)
        {
            _emailingConfiguration = emailingConfiguration;
        }

        public async Task SendAsync(IEmail email)
        {
            var message = new MimeMessage();
            message.To.AddRange(email.Addresses.Select(InternetAddress.Parse));
            message.From.Add(InternetAddress.Parse(_emailingConfiguration.SmtpUsername));

            message.Subject = email.Subject;
            //We will say we are sending HTML. But there are options for plaintext etc. 
            message.Body = new TextPart(TextFormat.Html)
            {
                Text = email.Content
            };

            //Be careful that the SmtpClient class is the one from Mailkit not the framework!
            using (var emailClient = new SmtpClient())
            {
                //The last parameter here is to use SSL (Which you should!)
                emailClient.Connect(_emailingConfiguration.SmtpServer, _emailingConfiguration.SmtpPort);

                //Remove any OAuth functionality as we won't be using it. 
                emailClient.AuthenticationMechanisms.Remove("XOAUTH2");

                emailClient.Authenticate(_emailingConfiguration.SmtpUsername, _emailingConfiguration.SmtpPassword);

                await emailClient.SendAsync(message);

                emailClient.Disconnect(true);
            }
        }
    }
}
