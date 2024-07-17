using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ServiceListening
{
    public class process
    {
        public void sendmail()
        {
            try
            {
                using (SmtpClient client = new SmtpClient("sandbox.smtp.mailtrap.io", 587))
                {
                    client.EnableSsl = false;
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential("bfa7eea3ac7657", "6e4fae0f79642d");

                    MailMessage message = new MailMessage();
                    message.From = new MailAddress("send@serviciosespeciales.com.mx");
                    message.To.Add("jvargas@exosfera.com");
                    message.Subject = "Test Email";
                    message.Body = "hello world";

                    client.Send(message);
                }
                //var client = new SmtpClient("sandbox.smtp.mailtrap.io", 25)
                //{
                //    Credentials = new NetworkCredential("bfa7eea3ac7657", "6e47fae0f79642d"),
                //    EnableSsl = true
                //};

                //client.Send("send@serviciosespeciales.com.mx", "jvargas@exosfera.com", "Envio de mensaje hl7", "Testbody");
            }
            catch (Exception ex)
            {
                string pathfileerr = @"c:\messages\errors.txt";
                using (StreamWriter writer = new StreamWriter(pathfileerr))
                {
                    writer.WriteLine(ex.Message.ToString());
                }
            }
        }

        public void sendjson(String message)
        {
            // URL a la que deseas enviar la petición POST
            string url = "https://serviciosespeciales.com.mx/input_messagge.pl";

            try
            {
                // Crear la solicitud HTTP POST
                HttpWebRequest solicitud = (HttpWebRequest)WebRequest.Create(url);
                solicitud.Method = "POST";
                solicitud.ContentType = "application/json";

                // Datos que deseas enviar en el cuerpo de la petición
                string datos = "{ \"action\": \"send_notification\" }"; ;

                // Convertir los datos a un array de bytes
                byte[] datosBytes = Encoding.UTF8.GetBytes(datos);
                solicitud.ContentLength = datosBytes.Length;

                // Escribir los datos en el cuerpo de la petición
                using (Stream stream = solicitud.GetRequestStream())
                {
                    stream.Write(datosBytes, 0, datosBytes.Length);
                }

                // Obtener la respuesta del servidor
                //using (HttpWebResponse respuesta = (HttpWebResponse)solicitud.GetResponse())
                //{
                //    using (Stream respuestaStream = respuesta.GetResponseStream())
                //    {
                //        StreamReader reader = new StreamReader(respuestaStream, Encoding.GetEncoding("utf-8"));
                //        string respuestaContenido = reader.ReadToEnd();
                //        Console.WriteLine("Respuesta del servidor: " + respuestaContenido);
                //    }
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al enviar la petición: " + ex.Message);
            }
        }

        public void sendnotification()
        {
            string url = "https://serviciosespeciales.com.mx/input_message.pl";

            //try
            //{
                HttpWebRequest solicitud = (HttpWebRequest)WebRequest.Create(url);
                solicitud.Method = "POST";
                solicitud.ContentType = "application/x-www-form-urlencoded";

                string datos = "action=send_notification";

                byte[] datosBytes = Encoding.UTF8.GetBytes(datos);
                solicitud.ContentLength = datosBytes.Length;

                using (Stream stream = solicitud.GetRequestStream())
                {
                    stream.Write(datosBytes, 0, datosBytes.Length);
                }
            //}
            //catch (Exception ex)
            //{
            //    string pathfileerr = @"c:\messages\errors.txt";
            //    using (StreamWriter writer = new StreamWriter(pathfileerr))
            //    {
            //        writer.WriteLine(ex.Message.ToString());
            //    }
            //}
        }

        public async Task sendjsonasync()
        {
            // URL a la que deseas enviar la petición POST
            string url = "https://ejemplo.com/api";

            // Datos que deseas enviar en el cuerpo de la petición
            var datos = new { campo1 = "valor1", campo2 = "valor2" };

            // Convertir los datos a formato JSON
            var contenido = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(datos), System.Text.Encoding.UTF8, "application/json");

            // Crear una instancia de HttpClient
            using (HttpClient cliente = new HttpClient())
            {
                try
                {
                    // Enviar la petición POST y obtener la respuesta
                    HttpResponseMessage respuesta = await cliente.PostAsync(url, contenido);

                    // Verificar si la petición fue exitosa (código de estado 200-299)
                    if (respuesta.IsSuccessStatusCode)
                    {
                        // Leer la respuesta
                        string contenidoRespuesta = await respuesta.Content.ReadAsStringAsync();
                        Console.WriteLine("Respuesta del servidor: " + contenidoRespuesta);
                    }
                    else
                    {
                        Console.WriteLine("Error al enviar la petición. Código de estado: " + respuesta.StatusCode);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
        }
    }
}
