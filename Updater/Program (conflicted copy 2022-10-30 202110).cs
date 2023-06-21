using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Updater
{
    internal class Program
    {
        WebClient client = new WebClient();
        static void Main(string[] args)
        {
            if (File.Exists("WIndowsImageDeployerPE.exe"))
            {
                File.Delete("WIndowsImageDeployerPE.exe");

            }
            Updater.Program program = new Updater.Program();
            program.DownloadUpdate();
        }
        public void DownloadUpdate()
        {
            client.Headers.Add("user-agent", "Mozilla/5.0 AppleWebKit/537.36 (KHTML, like Gecko; compatible; Googlebot/2.1; +http://www.google.com/bot.html) Safari/537.36");
            client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadProgressCallback4);
            client.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadFileCallback2);
            client.DownloadFileAsync(new Uri(GetDownloadLink()), "WindowsImageDeployerPE.exe");
            Console.ReadKey();
        }
        private static void DownloadProgressCallback4(object sender, DownloadProgressChangedEventArgs e)
        {

            Console.Write("Downloaded Bytes: {0}\r", e.BytesReceived + " Out of " + e.TotalBytesToReceive);

        }
        private static void DownloadFileCallback2(object sender, AsyncCompletedEventArgs ee)
        {
            if (ee.Cancelled)
            {
                Console.WriteLine("File download cancelled.");
            }

            else if (ee.Error != null)
            {
                Console.WriteLine(ee.Error.ToString());
            }
            else
            {
                Console.WriteLine("\nRebooting");
                Process process = new Process();
                process.StartInfo.FileName = "wpeutil.exe";
                process.StartInfo.Arguments = "reboot";
                process.Start();
               

                string output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();

            }
            



        }

        public string GetDownloadLink()
        {
            client.Headers.Add("user-agent", "Mozilla/5.0 AppleWebKit/537.36 (KHTML, like Gecko; compatible; Googlebot/2.1; +http://www.google.com/bot.html) Safari/537.36");

            string result = " ";

            JToken token = JToken.Parse(client.DownloadString("https://api.github.com/repos/cj1705/Windows11Deployer/releases/latest"));
  
           
            JArray value = (JArray)token.SelectToken("assets");
            foreach(JToken m in value)
            {
              
               result = (string) m["browser_download_url"];
                
            }

            return result;


        }
    }
}
