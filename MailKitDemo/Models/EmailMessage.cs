﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MailKitDemo.Models
{
    public class EmailMessage
    {
		public EmailMessage()
		{
			ToAddresses = new List<EmailAddress>();
			FromAddresses = new List<EmailAddress>();
		}
	
		public List<EmailAddress> ToAddresses { get; set; }
		public List<EmailAddress> FromAddresses { get; set; }
		public string Subject { get; set; } = string.Empty;
		public string Content { get; set; } = string.Empty;
	}
}
