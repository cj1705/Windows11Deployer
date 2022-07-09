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
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WIndowsImageDeployerPE.forms
{
    public partial class _403 : Form
    {
     
        public _403()
        {
            InitializeComponent();
          
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void _403_Load(object sender, EventArgs e)
        {
            WebClient webClient = new WebClient();
            richTextBox1.Text = webClient.DownloadString("https://projects.carsongames.com/windows11deployer/403.txt");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            WebClient wc = new WebClient();

            try
            {
                using (Stream stream  = wc.OpenRead("https://projects.carsongames.com/windows11deployer/list.txt"))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string line;

                       line = reader.ReadLine();
                        Process.Start(line.Split(';')[1].ToString());


                    }
                }
            }
            catch (WebException e1)
            {
                Console.WriteLine("Error! - " + e1.Message);
                this.Close();
            }

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void _403_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();

        }
    }
}
