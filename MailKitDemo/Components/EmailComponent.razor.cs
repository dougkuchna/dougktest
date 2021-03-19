using MailKitDemo.Contracts;
using MailKitDemo.Models;
using MailKitDemo.Models.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MailKitDemo.Components
{
    public partial class EmailComponent
    {
        [Inject]
        IEmailService emailService { get; set; }
        protected EmailAddress EmailAddress { get; set; } = new EmailAddress();
        protected EmailMessage EmailMessage { get; set; } = new EmailMessage();



        protected Dictionary<string, object> InputTextAreaAttributes { get; set; } =
            new Dictionary<string, object>()
            {
            { "class", "k-textbox email-content" }

            };

        public void SendEmail()
        {
            EmailAddress.Address = "dougkuchna@gmail.com";
            EmailMessage.Subject = "test";
            EmailMessage.Content = "This is a test of MailKit and hMailServer";
            EmailMessage.ToAddresses.Add(EmailAddress);
            EmailMessage.FromAddresses.Add(new EmailAddress { Address = "dkuchna@phimail.databasics.com" });
            try
            {
                emailService.Send(EmailMessage);
            }
            catch (Exception e)
            {

                throw;
            }
     
            
        }

        public void ReceiveEmail()
        {
            List<EmailMessage> msgs;
            try
            {
                msgs = emailService.ReceiveEmail();
            }
            catch (Exception e)
            {

                throw;
            }

            for(int i = 0; i< msgs.Count; i++)
            {

            }
        }
    }
}
