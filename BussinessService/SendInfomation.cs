using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
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
        public void Writetxt(string data = "", bool typelog = true)
        {
            DateTime dtnow = new DateTime();
            Random rand = new Random();
            string pathfile = "";

            pathfile = @"c:\messages\message" + rand.Next(10, 101).ToString() + ".txt";

            if (!typelog)
            {
                pathfile = @"c:\messages\errors" + rand.Next(10, 101).ToString() + ".txt";
            }

            using (StreamWriter writer = new StreamWriter(pathfile))
            {
                writer.WriteLine(data);
            };
        }
        public void Sendrequest(string message = "")
        {
            string url = "https://serviciosespeciales.com.mx/input_messagge.pl";

            HttpWebRequest solicitud = (HttpWebRequest)WebRequest.Create(url);
            solicitud.Method = "POST";
            solicitud.ContentType = "application/json";

            string datos = "{ \"action\": \"process_message\", \"message\":\"" + message + "\"}";

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
        public string Sendbyport()
        {
            string messagereturn = string.Empty;
            
            string filepath = "c:/messages/mensaje.txt";

            string content = File.ReadAllText(filepath);

            string url = "http://localhost:120?mensaje="+content;

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
        public byte[] Compress(string msj)
        {
            byte[] stringbytes = Encoding.UTF8.GetBytes(msj);

            using (MemoryStream mstream = new MemoryStream())
            {
                using (GZipStream zipstream = new GZipStream(mstream, CompressionMode.Compress, true))
                {
                    zipstream.Write(stringbytes, 0, stringbytes.Length);
                }

                return mstream.ToArray();
            }
        }

        public string Reduce(string msj)
        {
            string evaluate = msj.Trim();

            evaluate = Regex.Replace(evaluate, @"\s+", "");
            evaluate = Regex.Replace(evaluate, @"\\", "");

            return evaluate;
        }

        public void Sendmessagerequest(string message = "")
        {
            string url = "https://serviciosespeciales.com.mx/input_messagge.pl";

            HttpWebRequest solicitud = (HttpWebRequest)WebRequest.Create(url);
            solicitud.Method = "POST";
            solicitud.ContentType = "application/json";

            using (HttpClient client = new HttpClient())
            {
                string datos = "{ \"action\": \"process_message\", \"message\":\"" + message + "\"}";
                
                StringContent cont = new StringContent(datos, Encoding.UTF8, "text/xml");

                HttpResponseMessage response = client.SendAsync(new HttpRequestMessage(HttpMethod.Post, url){
                    Content = cont
                }).Result;

                string responsecontent = response.Content.ReadAsStringAsync().Result;

                string a = string.Empty;
            }
        }

        public void Sendmessagerequestv2(string message = "")
        {
            string url = "https://serviciosespeciales.com.mx/input_messagge.pl";

            HttpWebRequest solicitud = (HttpWebRequest)WebRequest.Create(url);
            solicitud.Method = "POST";
            solicitud.ContentType = "application/json";

            message = message + "action | process_message";

            JObject hl7Json = ConvertHL7ToJson(message);

            //string datos = "{ \"action\": \"process_message\", \"message\":\"" + hl7Json.ToString(Formatting.Indented) + "\"}";
            //string datos = hl7Json.ToString(Formatting.Indented);
            string datos = "{ \"action\": \"process_message\", \"message\":" + hl7Json.ToString(Formatting.Indented) + "}";

            datos = Regex.Replace(datos, @"\n", "");

            byte[] datosBytes = Encoding.UTF8.GetBytes(datos);
            solicitud.ContentLength = datosBytes.Length;

            using (Stream stream = solicitud.GetRequestStream())
            {
                stream.Write(datosBytes, 0, datosBytes.Length);
            }

            using (HttpWebResponse response = (HttpWebResponse)solicitud.GetResponse())
            {
                using (Stream streamresponse = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(streamresponse, Encoding.GetEncoding("utf-8"));
                    string responsecontent = reader.ReadToEnd();

                }
            }
        }

        static JObject ConvertHL7ToJson(string hl7String)
        {

            // Dividir la cadena HL7 en segmentos

            string[] segments = hl7String.Split('\r');

            // Crear un objeto JSON

            JObject hl7Json = new JObject();

            // Recorrer los segmentos y convertirlos en propiedades JSON

            foreach (string segment in segments)
            {

                // Dividir el segmento en campos

                string[] fields = segment.Split('|');

                // Crear una matriz JSON para los campos del segmento

                JArray segmentFields = new JArray();

                foreach (string field in fields)
                {

                    segmentFields.Add(field);

                }

                // Agregar el segmento al objeto JSON

                hl7Json[fields[0]] = segmentFields;

            }

            return hl7Json;

        }
    }
}
