using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WIndowsImageDeployerPE
{
    public partial class progress : Form
    {
       
        public progress(string location)
        {
            try
            {
                InitializeComponent();
                WebClient client = new WebClient();
                client.DownloadFileCompleted += Client_DownloadFileCompleted;
                client.DownloadProgressChanged += Client_DownloadProgressChanged;
                client.DownloadFileAsync(new Uri("https://chaosityyoutube.com/carson/Windows11Deployer.iso"), location + "\\Windows11Deployer.iso");

               
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Application.Exit();
            }
            }

        private void Client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
          
            progressBar1.Value = e.ProgressPercentage;
            label1.Text = $"{e.BytesReceived} / {e.TotalBytesToReceive} Bytes";
            label2.Text = $"{e.ProgressPercentage}%";
        }

        private void Client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message);
                Application.Exit();
            }
                this.Close();
        }
      

        private void progress_Load(object sender, EventArgs e)
        {
           
        }
    }
}
