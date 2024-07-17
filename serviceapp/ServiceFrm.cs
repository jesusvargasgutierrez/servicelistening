using BussinessService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Serviceapp
{
    public partial class ServiceFrm : Form
    {
        SendInfomation BussinessProccess;
        public ServiceFrm()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            BussinessProccess = new SendInfomation();

            string filepath = "c:/develop/m.txt";

            string content = File.ReadAllText(filepath);

            //string msj = "GET /?mensaje=testdev HTTP/1.1 \n Host: localhost:120 \n Connection: Keep-Alive";
            var r = BussinessProccess.Reduce(content);

            BussinessProccess.Sendrequest(r);
            //MessageBox.Show(BussinessProccess.Sendbyport());
        }
    }
}
