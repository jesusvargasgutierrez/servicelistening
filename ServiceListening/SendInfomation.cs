using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BussinessService
{
    public class SendInfomation
    {
        public void Sendmail(string BodyMessage = "")
        {
            var client = new SmtpClient("smtp.mandrillapp.com", 587)
            {
                Credentials = new NetworkCredential("apps@exosoluciones.com", "md-0vOB4VJFtFM526EjJD3nWA"),
                EnableSsl = false
            };

            MailMessage message = new MailMessage();
            message.From = new MailAddress("info@serviciosespeciales.com.mx");
            message.To.Add("jvargas@exosfera.com");
            message.Subject = "Notificacion proceso";
            message.Body = BodyMessage;

            client.Send(message);
        }

        public void Sendrequest(String message = "")
        {
            string url = "https://serviciosespeciales.com.mx/input_messagge.pl";

            HttpWebRequest solicitud = (HttpWebRequest)WebRequest.Create(url);
            solicitud.Method = "POST";
            solicitud.ContentType = "application/json";

            string formatmessage = Reduce(message);

            string datos = "{ \"action\": \"process_message\", \"message\":\"" + formatmessage + "\"}";

            byte[] datosBytes = Encoding.UTF8.GetBytes(datos);
            solicitud.ContentLength = datosBytes.Length;

            using (Stream stream = solicitud.GetRequestStream())
            {
                stream.Write(datosBytes, 0, datosBytes.Length);
            }

            using (HttpWebResponse response =  (HttpWebResponse) solicitud.GetResponse())
            {
                using (Stream streamresponse = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(streamresponse, Encoding.GetEncoding("utf-8"));
                    string responsecontent = reader.ReadToEnd();

                }
            }
        }

        public void Writetxt(string data = "", bool typelog = true)
        {
            DateTime dtnow = new DateTime();
            Random rand = new Random();
            string pathfile = "";

            pathfile = @"c:\messages\message" + rand.Next(10,101).ToString() + ".txt";

            if (!typelog)
            {
                pathfile = @"c:\messages\errors_service" + rand.Next(10, 101).ToString() + ".txt";
            }

            using (StreamWriter writer = new StreamWriter(pathfile))
            {
                writer.WriteLine(data);
            };
        }

        public string Writetxtreturn(string data = "", bool typelog = true)
        {
            DateTime dtnow = new DateTime();
            Random rand = new Random();
            string pathfile = "";

            pathfile = @"c:\messages\message" + rand.Next(10, 101).ToString() + ".txt";

            if (!typelog)
            {
                pathfile = @"c:\messages\errors_service" + rand.Next(10, 101).ToString() + ".txt";
            }

            using (StreamWriter writer = new StreamWriter(pathfile))
            {
                writer.WriteLine(data);
            };

            return pathfile;
        }
        public string Reduce(string msj)
        {
            string evaluate = msj.Trim();

            evaluate = Regex.Replace(evaluate, @"\s+", "");
            evaluate = Regex.Replace(evaluate, @"\\", "");
            evaluate = Regex.Replace(evaluate, @"<0x0b>", "");
            evaluate = Regex.Replace(evaluate, @"<0x1c>", "");
            evaluate = Regex.Replace(evaluate, @"", "");

            return evaluate;
        }
        public string Sendbyport()
        {
            string messagereturn = string.Empty;

            string filepath = "c:/messages/mensaje.txt";

            string content = File.ReadAllText(filepath);

            string url = "http://localhost:120?mensaje=" + Uri.EscapeDataString(content);

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";

                using (WebResponse response = request.GetResponse())
                {
                    using (Stream datastream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(datastream);
                        messagereturn = reader.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
                messagereturn = ex.Message.ToString();
            }

            return messagereturn;
        }
    }
}
