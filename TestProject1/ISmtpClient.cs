using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace TestProject1
{
    public interface ISmtpClient
    {
        void Send(MailMessage message);
    }
}
