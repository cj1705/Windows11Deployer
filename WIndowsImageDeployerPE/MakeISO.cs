using DiscUtils;
using DiscUtils.Iso9660;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WIndowsImageDeployerPE
{
    public partial class MakeISO : Form
    {
        private long fileSize;    //the size of the zip file
        private long extractedSizeTotal;    //the bytes total extracted
        private long compressedSize;    //the size of a single compressed file
        private string compressedFileName;    //the name of the file being extracte
        private BackgroundWorker extractFile;
        
        DriveInfo[] allDrives = DriveInfo.GetDrives();
        DriveInfo drive;

       public string DriveLetter = " ";
        public MakeISO()
        {
            InitializeComponent();
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
        Thread start = new Thread(Format);
            start.Start();
        }
        public void Append(string text)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => Append(text)));
            }
            else
            {
                richTextBox1.AppendText("\n" + text);
            }
        }
        public void DownloadISO()
        {
            Append("Downloading ISO ");
         WebClient wc = new WebClient();
            if (progressBar1.InvokeRequired)
            {
                progressBar1.Invoke(new Action(() => progressBar1.Visible = true));
            }
            if (File.Exists(Path.GetTempPath() + "/Windows11Deployer.iso"))
            {
                File.Delete(Path.GetTempPath() + "/Windows11Deployer.iso");
            }

            wc.DownloadProgressChanged += Wc_DownloadProgressChanged;
            wc.DownloadFileAsync(new Uri("https://chaosityyoutube.com/Windows11Deployer.iso"), Path.GetTempPath() + "/Windows11Deployer.iso");
            wc.DownloadFileCompleted += Wc_DownloadFileCompleted;

        }

        private void Wc_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (progressBar1.InvokeRequired)
            {
                progressBar1.Invoke(new Action(() => progressBar1.Visible = false));
            }
            if (label1.InvokeRequired)
            {
                label1.Invoke(new Action(() => label1.Text = ""));
            }
          
        }

        private void Wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            if (InvokeRequired)
            {
                if (progressBar1.InvokeRequired)
                {
                    progressBar1.Invoke(new Action(() => progressBar1.Value = e.ProgressPercentage));
                }
                if (label1.InvokeRequired)
                {
                    label1.Invoke(new Action(() => label1.Text = e.BytesReceived + " / " + e.TotalBytesToReceive + " Bytes"));
                }
            }

        }

        public void Format()
        {
            Append("Formatting " + DriveLetter);
            Process process = new Process();
            process.StartInfo.FileName = "cmd";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = false;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.RedirectStandardInput = true;
            process.Start();
           process.StandardInput.WriteLine($"format /y {DriveLetter.Substring(0,2)} /fs:NTFS /v:WIN11 /q");

            process.StandardInput.WriteLine($"exit");

            process.WaitForExit();
            //DownloadISO();
            Extract();
            



        }

        private void MakeISO_Load(object sender, EventArgs e)
        {

            foreach (DriveInfo d in allDrives)
            {
                if (d.DriveType == DriveType.Removable && d.IsReady)
                {
                    DeviceBox.Items.Add(d.Name + " - " + d.VolumeLabel);
                    drive = d;
                 
                    
 
                }


            }
            if (DeviceBox.Items.Count == 0)
            {
                DeviceBox.Text = "No Devices Found";
                DeviceBox.Enabled = false;

            }
            else
            {
                DeviceBox.SelectedIndex = 0;
                DeviceBox.Enabled = true;
                DeviceBox.Text = DeviceBox.Items[0].ToString();
            }
        }

        public void Extract()
        {
            extract extract = new extract("H:\\VS stuff\\Projects\\CarsonGamesGeos\\CarsonGamesGeos\\bin\x86\\Debug\\es-es.zip",DriveLetter);

        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            DriveLetter = DeviceBox.SelectedItem.ToString().Split('-')[0];
        }
        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            DriveLetter = DeviceBox.SelectedItem.ToString().Split('-')[0];
        }
        private void label1_Click(object sender, EventArgs e)
        {
            DriveLetter = DeviceBox.SelectedItem.ToString().Split('-')[0];
        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }
    }

    
}
