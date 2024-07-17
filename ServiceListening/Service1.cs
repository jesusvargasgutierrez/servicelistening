using BussinessService;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.ServiceProcess;
using System.Text;
//using System.Threading;
//using System.Threading.Tasks;
using System.Timers;
using System.Web;

namespace ServiceListening
{
    public partial class Service1 : ServiceBase
    {
        private TcpListener listener;
        private int port = 120;

        private readonly HttpClient httpClient;
        private Timer timer;
        public Service1()
        {
            InitializeComponent();

        }

        protected override void OnStart(string[] args)
        {
            //try
            //{
            //    timer = new Timer();
            //    timer.Interval = TimeSpan.FromMinutes(2).TotalMilliseconds;
            //    timer.Elapsed += sendinfo;
            //    timer.Start();
            //}
            //catch (Exception ex)
            //{
            //    string pathfileerr = @"c:\messages\errors.txt";
            //    using (StreamWriter writer = new StreamWriter(pathfileerr))
            //    {
            //        writer.WriteLine(ex.Message.ToString());
            //    }
            //}

            listener = new TcpListener(IPAddress.Any, port);
            listener.Start();
            listener.BeginAcceptTcpClient(new AsyncCallback(HandleClient), null);
        }

        private void HandleClient(IAsyncResult result)
        {
            TcpClient client = listener.EndAcceptTcpClient(result);
            //NetworkStream stream = client.GetStream();

            //string dataReceived = string.Empty;

            //using (MemoryStream ms = new MemoryStream())
            //{
            //    byte[] buffer = new byte[9144];

            //    int byteread;
            //    while ((byteread = stream.Read(buffer, 0, buffer.Length)) > 0)
            //    {
            //        ms.Write(buffer, 0, byteread);
            //    }

            //    dataReceived = Encoding.UTF8.GetString(ms.ToArray());
            //}

            byte[] buffer = new byte[9144];
            int bytesRead = client.GetStream().Read(buffer, 0, buffer.Length);
            string dataReceived = Encoding.UTF8.GetString(buffer, 0, bytesRead);

            SendInfomation BussinessProccess = new SendInfomation();

            //string sanitizate = BussinessProccess.Reduce(dataReceived);
            //dataReceived = Uri.UnescapeDataString(sanitizate);

            try
            {
                string path = BussinessProccess.Writetxtreturn(dataReceived);
                string content = File.ReadAllText(path);
                string sanitizate = BussinessProccess.Reduce(content);

                BussinessProccess.Sendrequest(sanitizate);
                //BussinessProccess.Sendmail("Un archivo se ha procesado");
            }
            catch (Exception ex)
            {
                BussinessProccess.Writetxt(ex.Message.ToString(), false);
                //BussinessProccess.Sendmail(ex.Message.ToString());
            }

            client.Close();
            listener.BeginAcceptTcpClient(new AsyncCallback(HandleClient), null);
        }
        protected override void OnStop()
        {
            //if(listener != null)
            //{
            //    listener.Stop();
            //}
        }
    }
}
