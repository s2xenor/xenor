/*
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Runtime.InteropServices;
using IWshRuntimeLibrary;
using Octokit;
using File = System.IO.File;

namespace AlteraVita
{
    class Program
    {
        static void Main(string[] args)
        {
            // Install("D:\\pc\\Bureau");
            // Update();
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return;
            }

            while (true)
            {
            
            
                Console.WriteLine("What do you want to do ?");
                Console.WriteLine("1) Install Altera Vita");
                Console.WriteLine("2) Update Altera Vita");
                Console.WriteLine("3) Uninstall Altera Vita");
                Console.WriteLine("4) Cancel");
                Console.WriteLine("Enter the number corresponding to desired action.");
            
                string r;
                bool flag = false;
                int n;
                do
                {
                    r = Console.ReadLine();
                    if (int.TryParse(r, out n))
                    {
                        if (0 < n && n < 4) flag = true;
                        else Console.Error.WriteLine($"{n} is not a valid option, please try again.");
                    }
                    else Console.Error.WriteLine($"{r} is not a valid number, please try again.");
                } while (!flag);
            
                ChooseOption(n);
            }
        }

        private static void ChooseOption(int n)
        {
            string r = "C:\\Program Files\\AlteraVita\\";
            bool flag = false;
            switch (n)
            {
                case 1:
                    Console.Clear();
                    Console.WriteLine("Where do you want to install Altera Vita ?");
                    Console.WriteLine("Press enter to default (C:\\Program Files\\AlteraVita\\).");
                    Console.WriteLine("Press c (and enter) to cancel.");
                    while (!flag)
                    {
                        r = Console.ReadLine();
                        if (r == "")flag = true;
                        else if (Directory.Exists(r)) flag = true;
                        else Console.Error.WriteLine("The directory does not exist, please try again.");
                    }

                    Install(r);
                    break;
                case 2:
                    Update();
                    break;
                case 3:
                    Uninstall();
                    break;
                case 4:
                    Environment.Exit(0);
                    break;
            }
        }

        private static async void Install(string destination)
        {
            Console.WriteLine("Fetching the last version of Altera vita from the server...");
            GitHubClient client = new GitHubClient(new ProductHeaderValue("hugueprime"));
            IReadOnlyList<Release> releases = await client.Repository.Release.GetAll("s2xenor", "xenor");
            var latest = releases[0];
            
            Console.WriteLine("Downloading...");
            
            WebClient webClient = new WebClient();
            webClient.Headers.Add("user-agent", "Anything");
            await webClient.DownloadFileTaskAsync(new Uri(latest.Assets[0].BrowserDownloadUrl), destination+"/tmp.zip");
           
            Console.WriteLine("Downloading finished.");
            Console.WriteLine("Creating folder AlteraVita.");
            
            Directory.CreateDirectory(destination + "/AlteraVita");
            
            Console.WriteLine("Extracting game...");
            ZipFile.ExtractToDirectory(destination+"/tmp.zip", destination + "/AlteraVita");
            Console.WriteLine("Extracting finished.");
            
            Console.WriteLine("Cleaning...");
            File.Delete(destination+"/tmp.zip");
            Console.WriteLine("Cleaning done.");
            
            Console.WriteLine("Add shortcut to desktop...");
            WshShellClass wsh = new WshShellClass();
            IWshRuntimeLibrary.IWshShortcut shortcut = wsh.CreateShortcut(
                Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\AlteraVita.lnk") as IWshRuntimeLibrary.IWshShortcut;
            shortcut.Arguments = "";
            shortcut.TargetPath = $"{destination}\\AlteraVita\\AlteraVita.exe";
            // not sure about what this is for
            shortcut.WindowStyle = 1; 
            shortcut.Description = "my shortcut description";
            shortcut.WorkingDirectory = "c:\\app";
            shortcut.IconLocation = $"{destination}\\AlteraVita\\";
            shortcut.Save();
            Console.WriteLine("Shortcut added.");
            
            
            Console.WriteLine("Adding keys to registry...");
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Microsoft.Win32.RegistryKey key;
                key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"SOFTWARE\AlteraVita");
                if (key == null)
                {
                    key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(@"SOFTWARE\AlteraVita");
                }
                key.SetValue("version", latest.CreatedAt); 
                key.SetValue("path", destination);
                key.Close(); 
            }
            
            Console.WriteLine("Keys added to registry.");

            
            Console.Clear();
            Console.WriteLine("Successfully installed !");
            Console.WriteLine("Press any key to exit");
            Console.Read();

        }

        private static async void Update()
        {
            Console.WriteLine("Fetching current version in registry.");
        
            Microsoft.Win32.RegistryKey key;
            key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"SOFTWARE\AlteraVita");
            if (key == null)
            {
                Console.WriteLine("AltervaVita is currently not installed.");
                Console.WriteLine("Installing...");
                ChooseOption(1);
                return;
            }
            
            DateTimeOffset oldVersion = (DateTimeOffset) key.GetValue("version"); 
            string oldPath = (string) key.GetValue("path");
            
            Console.WriteLine("Fetching the last version of Altera vita from the server...");
            GitHubClient client = new GitHubClient(new ProductHeaderValue("SomeName"));
            IReadOnlyList<Release> releases = await client.Repository.Release.GetAll("s2xenor", "xenor");
            var latest = releases[0];
            
            if(latest.CreatedAt <= oldVersion)
            {
                Console.WriteLine("There is no newer version than your current one.");
                return;
            }
            
            
            Console.WriteLine("Downloading...");
            
            WebClient webClient = new WebClient();
            webClient.Headers.Add("user-agent", "Anything");
            await webClient.DownloadFileTaskAsync(new Uri(latest.Assets[0].BrowserDownloadUrl), oldPath+"/tmp.zip");
           
            Console.WriteLine("Downloading finished.");
            
            Console.WriteLine("Extracting game...");
            ZipFile.ExtractToDirectory(oldPath+"/tmp.zip", oldPath + "/AlteraVita");
            Console.WriteLine("Extracting finished.");
            
            Console.WriteLine("Cleaning...");
            File.Delete(oldPath+"/tmp.zip");
            Console.WriteLine("Cleaning done.");
            
            
            
            Console.WriteLine("Update keys to registry...");
            key.SetValue("path", latest.CreatedAt);
            key.Close(); 

            Console.WriteLine("Registry keys updated.");
            
            Console.Clear();
            Console.WriteLine("Successfully updated !");
            Console.WriteLine("Press any key to exit");
            Console.Read();

        }

        private static void Uninstall()
        {
            Microsoft.Win32.RegistryKey key;
            key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"SOFTWARE\AlteraVita");
            if (key == null)
            {
                Console.WriteLine("AltervaVita is currently not installed.");
                Console.WriteLine("Press any key to exit");
                Console.Read();
                return;
            }
            key.Close();

            String oldPath = (string) key.GetValue("path");
            Console.WriteLine("Deleting Alteravita...");
            Directory.Delete(oldPath);
            Console.WriteLine("Alteravita deleted.");

            Console.WriteLine("Deleting shortcut if there is...");
            if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\AlteraVita.lnk"))
            {
                File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\AlteraVita.lnk");
            }
            Console.WriteLine("Done.");
            Console.WriteLine("Cleaning registry...");
            Microsoft.Win32.Registry.CurrentUser.DeleteSubKey(@"SOFTWARE\AlteraVita");
            Console.WriteLine("Done.");
            Console.Clear();
            Console.WriteLine("Uninstall complete !");
            Console.WriteLine("Press any key to ");
            Console.Read();
        }
    }

}
*/