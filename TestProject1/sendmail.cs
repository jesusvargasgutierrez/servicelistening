﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace TestProject1
{
    public class sendmail
    {
        private readonly ISmtpClient _smtpClient;
        public sendmail(ISmtpClient smtpClient)
        {
            _smtpClient = smtpClient;
        }
        public void Sendmail()
        {
            MailMessage message = new MailMessage();
            message.From = new MailAddress("send@serviciosespeciales.com.mx");
            message.To.Add("jvargas@exosfera.com");
            message.Subject = "Test Email";
            message.Body = "hello!";

            _smtpClient.Send(message);
        }
    }
}
