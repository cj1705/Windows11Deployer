using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WIndowsImageDeployerPE
{
    public partial class progress : Form
    {
        string loc;
        public progress()
        {

            InitializeComponent();

            Console.WriteLine("\nSelect a folder to save the ISO");
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.SelectedPath = System.IO.Directory.GetCurrentDirectory();
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                loc = folderBrowserDialog.SelectedPath;
                if (File.Exists(folderBrowserDialog.SelectedPath + "\\Windows11Deployer.iso"))
                {
                    DialogResult dialogResult = MessageBox.Show("ISO Exists at this location! Overwrite?", "ISO Exists", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        File.Delete(folderBrowserDialog.SelectedPath + "\\Windows11Deployer.iso");
                        while (File.Exists(folderBrowserDialog.SelectedPath + "\\Windows11Deployer.iso")) { }
                    }

                  


                }
                Console.WriteLine(loc);
            }






            }

            private void Client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {


        }

        private void Client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message);
                Application.Exit();
            }
            MessageBox.Show("Download Finished!");

            Environment.Exit(0);
        }


        private void progress_Load(object sender, EventArgs e)
        {


            WebClient wc = new WebClient();
            //string lines = wc.DownloadString("https://chaosityyoutube.com/carson/Windows-11-deployer/list.txt");
            //foreach (var line in lines){
            //    ListViewItem item = new ListViewItem(line.ToString().Split(':')[0].ToString());

            //    item.SubItems.Add(line.ToString().Split(':')[1].ToString());
            //}
            try
            {
                using (Stream stream = wc.OpenRead("https://projects.carsongames.com/windows11deployer/list.txt"))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string line;

                        while ((line = reader.ReadLine()) != null)
                        {



                            ListViewItem listViewItem = new ListViewItem(line.Split(';')[0].ToString());
                            listViewItem.SubItems.Add(line.Split(';')[1].ToString());
                            listView1.Items.Add(listViewItem);

                        }


                    }
                }
            }
            catch (WebException e1)
            {
                Console.WriteLine("Error! - " + e1.Message);
                this.Close();
            }
            
        
    }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
              
                FileDownloader client = new FileDownloader();
                client.DownloadFileCompleted += Client_DownloadFileCompleted;
                client.DownloadProgressChanged += Client_DownloadProgressChanged1;
                client.DownloadFileAsync(listView1.SelectedItems[0].SubItems[1].Text, loc + $"\\Windows11Deployer.iso");
                button1.Enabled = false;
                listView1.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace);
            }
        }

        private void Client_DownloadProgressChanged1(object sender, FileDownloader.DownloadProgress e)
        {
            progressBar1.Value = e.ProgressPercentage;
            label1.Text = $"{e.BytesReceived} / {e.TotalBytesToReceive} Bytes";
            label2.Text = $"{e.ProgressPercentage}%";
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
