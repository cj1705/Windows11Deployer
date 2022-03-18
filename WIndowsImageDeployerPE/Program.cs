﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;

namespace WIndowsImageDeployerPE
{
    internal class Program
    {
       

      public  static void Main(string[] args)
        {
            
            WIndowsImageDeployerPE.Program program = new WIndowsImageDeployerPE.Program();
            List<string> drive_array = new List<string>();
            int driveindex;
            int drivesel;
            string drivename = " ";
            while (true)
            {
                Console.Clear();
                Console.WriteLine("--------Windows 11 Deployer--------");
                Console.WriteLine("Created by Carson Games");
                Console.WriteLine("-----------------------------------");

                Console.WriteLine("Getting Disks.");


            //    DriveInfo[] drives = DriveInfo.GetDrives();
            //    foreach (DriveInfo drive in drives)
            //    {
            //        if (drive.IsReady)
            //        {
            //            string label = drive.VolumeLabel;
            //            if (label == "" || label == " ")
            //            {
            //                label = "(no label)";
            //            }
            //            drive_array.Add(label + " - " + drive.TotalSize / 1000 / 1024 + "MB");
            //        }
            //    }
            //    foreach (string drive in drive_array)
            //    {
            //        Console.WriteLine("[" + drive_array.IndexOf(drive) + "] " + drive);

            //    }
            //    while (true)
            //    {
            //        try
            //        {
            //            Console.WriteLine("Pick a drive you would like to use.");
            //            int temp = Int32.Parse(Console.ReadLine());

            //            driveindex = temp;
            //            temp = 0;
            //            Console.WriteLine(drive_array[driveindex]);
            //            break;



            //        }
            //        catch (Exception e)
            //        {
            //            Console.WriteLine("Invalid Selection. Try again.");
            //        }
            //    }
            
            //    drivename = drive_array[driveindex].Split('-')[0];
            //    Console.WriteLine(drivename);
            //    Console.ReadLine();
            //}
            // execute DiskPart programatically
            Process process = new Process();
            process.StartInfo.FileName = "diskpart.exe";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.Start();
            process.StandardInput.WriteLine("list disk");
            process.StandardInput.WriteLine("exit");

                string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();


            // extract information from output
            string table = output.Split(new string[] { "DISKPART>" }, StringSplitOptions.None)[1];
            var rows = table.Split(new string[] { "\n" }, StringSplitOptions.None);
            for (int i = 3; i < rows.Length; i++)
            {
                if (rows[i].Contains("Disk"))
                {
                    int index = Int32.Parse(rows[i].Split(new string[] { " " }, StringSplitOptions.None)[3]);
                    string label = rows[i].Split(new string[] { " " }, StringSplitOptions.None)[16] + " "+rows[i].Split(new string[] { " " }, StringSplitOptions.None)[17];
                    long size = 0;


                    foreach (DriveInfo drive in DriveInfo.GetDrives())
                    {
                        
                        if(drive.IsReady && drive.VolumeLabel == label  )
                        {
                            size = (long)(drive.TotalSize / 1000 / 1024);
                        }
                    }
                    Console.WriteLine($@"Disk {index} {label}");
                }
            }
           
           
                Console.WriteLine("Select a disk number");
               drivesel = program.GetIndexOfDrive(Console.ReadLine());
                if (drivesel == -1)
                {
                    Console.WriteLine("Invalid Selection.");
                }
                else
                {
                    Console.WriteLine("Are you sure you want to continue with " + drivesel + "?");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("ALL DATA ON THAT DRIVE WILL BE FORMATTED!!!");
                    Console.ForegroundColor = ConsoleColor.White;
                    string answer = Console.ReadLine();
                    if (answer == "Y" || answer == "y")
                    {
                        
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Starting over");
                    }
                  

                }


              

            }

            Console.WriteLine(" (1/3) Formatting Drive");
            if (program.Format(drivesel))
            {
             
                Console.WriteLine("(2/3) Deploying Image - This may take awhile");
                if (program.Install(drivesel))
                {
                    
                    Console.WriteLine("(3/3) Adding BCD Records");
                    if (program.BCDRecords(drivesel))
                    {
                        Console.WriteLine("Windows 11 has been deployed! ");
                        Console.WriteLine("Run"+ " 'exit' "+" to reboot. ");
                       

                    }

                }

                Console.ReadLine();
            }






        }

       
        public void wc_DownloadProgressChanged(Object sender, DownloadProgressChangedEventArgs e)

        {
            Console.WriteLine("Downloading Image. - "+ e.ProgressPercentage + "% complete." );
            Console.SetCursorPosition(0, Console.CursorTop - 1);
           


        }
        public bool wc_DownloadProgressFinsihed(Object sender, DownloadStringCompletedEventArgs e)

        {

            Console.WriteLine("Completed!");


            return true;

        }



        public bool IsConnectedToInternet()
        {
            string host = "carsongames.com";  
            Ping p = new Ping();
            try
            {
                PingReply reply = p.Send(host, 3000);
                if (reply.Status == IPStatus.Success)
                    return true;
            }
            catch { }
            return false;
        }
       
        public bool Format(int index)
        {
            Process process = new Process();
            process.StartInfo.FileName = "diskpart.exe";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.Start();
            process.StandardInput.WriteLine("select disk " + index);
            process.StandardInput.WriteLine("clean");
            process.StandardInput.WriteLine("create partition primary size=100");
            process.StandardInput.WriteLine("format quick fs=ntfs label=System");
            process.StandardInput.WriteLine("assign letter=S");
            process.StandardInput.WriteLine("active");
            process.StandardInput.WriteLine("create partition primary");
            process.StandardInput.WriteLine("shrink minimum=650");
            process.StandardInput.WriteLine("format quick fs=ntfs label=Windows");
            process.StandardInput.WriteLine("assign letter=W");
            process.StandardInput.WriteLine("create partition primary");
            process.StandardInput.WriteLine("format quick fs=ntfs label=Recovery");
            process.StandardInput.WriteLine("assign letter=R");
            process.StandardInput.WriteLine("set id=27");
      
            process.StandardInput.WriteLine("exit");


            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            return true;
        }
        public bool Install(int index)
        {
            Process process = new Process();
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.FileName = "dism.exe";
            process.StartInfo.Arguments = "/apply-image /imagefile:install.wim /index:1 /ApplyDir:W:\\";
            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();


            return true;
        }
        public bool BCDRecords(int index)
        {
            Process process = new Process();
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.FileName = "x:\\windows\\system32\\bcdboot.exe";
            process.StartInfo.Arguments = " w:\\windows /s S: /f ALL";
            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            return true;
        }



        public int GetIndexOfDrive(string drive)
        {
            drive = drive.Replace(":", "").Replace(@"\", "");

            // execute DiskPart programatically
            Process process = new Process();
            process.StartInfo.FileName = "diskpart.exe";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.Start();
            process.StandardInput.WriteLine("list volume");
            process.StandardInput.WriteLine("exit");
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            // extract information from output
            string table = output.Split(new string[] { "DISKPART>" }, StringSplitOptions.None)[1];
            var rows = table.Split(new string[] { "\n" }, StringSplitOptions.None);
            for (int i = 3; i < rows.Length; i++)
            {
                if (rows[i].Contains("Volume"))
                {
                    int index = Int32.Parse(rows[i].Split(new string[] { " " }, StringSplitOptions.None)[3]);
                    string label = rows[i].Split(new string[] { " " }, StringSplitOptions.None)[3];

                    if (label.Equals(drive))
                    {
                        return index;
                    }
                }
            }

            return -1;
        }
    }
}
