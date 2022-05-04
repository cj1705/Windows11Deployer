using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WIndowsImageDeployerPE
{
    public class extract
    {
        MakeISO makeISO = (MakeISO)Application.OpenForms["OpenForms"];
        private BackgroundWorker extractFile;
        private long fileSize;    //the size of the zip file
        private long extractedSizeTotal;    //the bytes total extracted
        private long compressedSize;    //the size of a single compressed file
        private string compressedFileName;
        string filename = "H:\\VS stuff\\Projects\\CarsonGamesGeos\\CarsonGamesGeos\\bin\\x86\\Debug\\es-es.zip";
     




        public extract(string file,string drive)
        {
           

            //Set the maximum vaue to int.MaxValue, thus, it could be more accurate



            extractFile = new BackgroundWorker();
            extractFile.DoWork += ExtractFile_DoWork;
            extractFile.ProgressChanged += ExtractFile_ProgressChanged;
            extractFile.RunWorkerCompleted += ExtractFile_RunWorkerCompleted;
            extractFile.WorkerReportsProgress = true;
            extractFile.RunWorkerAsync();
            
        }

        private void ExtractFile_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //Set the maximum vaue to int.MaxValue because the process is completed
           
        }

        private void ExtractFile_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            makeISO.label1.Text = compressedFileName;

          

            //calculate the totalPercent
            long totalPercent = ((long)e.ProgressPercentage * compressedSize + extractedSizeTotal * int.MaxValue) / fileSize;
            if (totalPercent > int.MaxValue)
                totalPercent = int.MaxValue;
            makeISO.progressBar1.Value = (int)totalPercent;
        }

        private void ExtractFile_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
               
                
                //get the size of the zip file
                System.IO.FileInfo fileInfo = new System.IO.FileInfo(filename);
                fileSize = fileInfo.Length;
                using (Ionic.Zip.ZipFile zipFile = Ionic.Zip.ZipFile.Read(filename))
                {
                    //reset the bytes total extracted to 0
                    extractedSizeTotal = 0;
                    int fileAmount = zipFile.Count;
                    int fileIndex = 0;
                    zipFile.ExtractProgress += Zip_ExtractProgress;
                    foreach (Ionic.Zip.ZipEntry ZipEntry in zipFile)
                    {
                        fileIndex++;
                        compressedFileName = "(" + fileIndex.ToString() + "/" + fileAmount + "): " + ZipEntry.FileName;
                        //get the size of a single compressed file
                        compressedSize = ZipEntry.CompressedSize;
                        ZipEntry.Extract(makeISO.DriveLetter, Ionic.Zip.ExtractExistingFileAction.OverwriteSilently);
                        //calculate the bytes total extracted
                        extractedSizeTotal += compressedSize;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                //MessageBox.Show("An error occured while extraxting. Try again later.");
                //if (Properties.extractstuff.Default.type == "firsttime")
                //{
                //    MessageBox.Show("An error occured while downloading Internet Broswer. You can redownload it in the software center.");
                //}
            }
        }

        private void Zip_ExtractProgress(object sender, Ionic.Zip.ExtractProgressEventArgs e)
        {
            if (e.TotalBytesToTransfer > 0)
            {
                long percent = e.BytesTransferred * int.MaxValue / e.TotalBytesToTransfer;
                //Console.WriteLine("Indivual: " + percent);
                extractFile.ReportProgress((int)percent);
            }
        }

     
   


 

        
    }
}



