using MailKit.Net.Imap;
using MailKit.Net.Pop3;
using MailKit.Net.Smtp;
using MailKitDemo.Contracts;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MailKitDemo.Models
{
    public class EmailService : IEmailService
    {
        private readonly IEmailConfig _emailConfig;
        public EmailService(IEmailConfig emailConfig)
        {
            _emailConfig = emailConfig;
        }

        public List<EmailMessage> ReceiveEmail(int maxCount = 10)
        {
            using (Pop3Client emailClient = new Pop3Client())
            {
                emailClient.Connect(_emailConfig.PopServer, _emailConfig.PopPort, MailKit.Security.SecureSocketOptions.None);

                emailClient.AuthenticationMechanisms.Remove("XOAUTH2");

                emailClient.Authenticate(_emailConfig.PopUsername, _emailConfig.PopPassword);

                List<EmailMessage> emails = new List<EmailMessage>();
                for (int i = 0; i < emailClient.Count && i < maxCount; i++)
                {
                    var message = emailClient.GetMessage(i);
                    var emailMessage = new EmailMessage
                    {
                        Content = !string.IsNullOrEmpty(message.HtmlBody) ? message.HtmlBody : message.TextBody,
                        Subject = message.Subject
                    };
                    emailMessage.ToAddresses.AddRange(message.To.Select(x => (MailboxAddress)x).Select(x => new EmailAddress { Address = x.Address, Name = x.Name }));
                    emailMessage.FromAddresses.AddRange(message.From.Select(x => (MailboxAddress)x).Select(x => new EmailAddress { Address = x.Address, Name = x.Name }));
                    emails.Add(emailMessage);
                }

                return emails;
            }
        }

        public List<EmailMessage> GetEmail(i)
        {
            using (ImapClient emailClient = new ImapClient())
            {
                emailClient.Connect(_emailConfig.PopServer, _emailConfig.PopPort, MailKit.Security.SecureSocketOptions.None);

                emailClient.AuthenticationMechanisms.Remove("XOAUTH2");

                emailClient.Authenticate(_emailConfig.PopUsername, _emailConfig.PopPassword);

                List<EmailMessage> emails = new List<EmailMessage>();
                for (int i = 0; i < emailClient.Count; i++)
                {
                    var message = emailClient.GetMessage(i);
                    var emailMessage = new EmailMessage
                    {
                        Content = !string.IsNullOrEmpty(message.HtmlBody) ? message.HtmlBody : message.TextBody,
                        Subject = message.Subject
                    };
                    emailMessage.ToAddresses.AddRange(message.To.Select(x => (MailboxAddress)x).Select(x => new EmailAddress { Address = x.Address, Name = x.Name }));
                    emailMessage.FromAddresses.AddRange(message.From.Select(x => (MailboxAddress)x).Select(x => new EmailAddress { Address = x.Address, Name = x.Name }));
                    emails.Add(emailMessage);
                }

                return emails;
            }
        }

        public void Send(EmailMessage emailMessage)
        {
            MimeMessage message = new MimeMessage();
            message.To.AddRange(emailMessage.ToAddresses.Select(x => new MailboxAddress(x.Name, x.Address)));
            message.From.AddRange(emailMessage.FromAddresses.Select(x => new MailboxAddress(x.Name, x.Address)));

            message.Subject = emailMessage.Subject;
            //We will say we are sending HTML. But there are options for plaintext etc. 
            message.Body = new TextPart(TextFormat.Html)
            {
                Text = emailMessage.Content
            };

            //Be careful that the SmtpClient class is the one from Mailkit not the framework!
            using (var emailClient = new SmtpClient())
            {
                
                //   emailClient.ServerCertificateValidationCallback = (s, c, h, e) => true;//required for mailgun
                //The last parameter here is to use SSL (Which you should!)
                try
                {
                    emailClient.Connect(_emailConfig.SmtpServer, _emailConfig.SmtpPort, MailKit.Security.SecureSocketOptions.None);
                }
                catch (Exception e)
                {

                    throw;
                }
              

                //Remove any OAuth functionality as we won't be using it. 
                emailClient.AuthenticationMechanisms.Remove("XOAUTH2");
                //postmaster@ required for mailgun
                var test = $"postmaster@{_emailConfig.SmtpUsername}";
                emailClient.Authenticate(_emailConfig.SmtpUsername, _emailConfig.SmtpPassword);

                emailClient.Send(message);

                emailClient.Disconnect(true);
            }
        }


        //public List<EmailMessage> Receive()
        //{
        //  //  var x = new MailKit
            
        //    using (var emailClient = new Pop3Client())
        //    {

        //        //   emailClient.ServerCertificateValidationCallback = (s, c, h, e) => true;//required for mailgun
        //        //The last parameter here is to use SSL (Which you should!)
        //        try
        //        {
        //            emailClient.Connect(_emailConfig.PopServer, _emailConfig.PopPort, MailKit.Security.SecureSocketOptions.None);
        //        }
        //        catch (Exception e)
        //        {

        //            throw;
        //        }
        //        emailClient.Authenticate(_emailConfig.PopUsername, _emailConfig.PopPassword);

        //        List<EmailMessage> emails = new List<EmailMessage>();
        //        for (int i = 0; i < emailClient.Count && i < 10; i++)
        //        {
        //            var message = emailClient.GetMessage(i);
        //            var emailMessage = new EmailMessage
        //            {
        //                Content = !string.IsNullOrEmpty(message.HtmlBody) ? message.HtmlBody : message.TextBody,
        //                Subject = message.Subject
        //            };
        //            emailMessage.ToAddresses.AddRange(message.To.Select(x => (MailboxAddress)x).Select(x => new EmailAddress { Address = x.Address, Name = x.Name }));
        //            emailMessage.FromAddresses.AddRange(message.From.Select(x => (MailboxAddress)x).Select(x => new EmailAddress { Address = x.Address, Name = x.Name }));
        //            emails.Add(emailMessage);
        //        }

        //        return emails;
        //        //emailClient.GetMessagesAsync();
        //        //emailClient.
        //    }

        //}
    }
}
