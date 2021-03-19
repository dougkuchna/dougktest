using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MailKitDemo.Models.ViewModels
{
    public class EmailComponentModel
    {
        [Required]
        public EmailAddress emailAddress { get; set; } = new EmailAddress();
        [Required]
        public EmailMessage emailMessage { get; set; } = new EmailMessage();
    }
}
