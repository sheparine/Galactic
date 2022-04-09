using Galactic.DriveScanner.App.Models;
using Microsoft.Extensions.Configuration;

namespace Galactic.DriveScanner.App
{
    internal class Program
    {
        private static List<SearchPattern> SearchPatterns = new List<SearchPattern>();

        public static void Main(string[] args)
        {
            Console.WriteLine("Starting Drive Scanner");

            var appConfig = GetAppConfig();
            SearchPatterns = appConfig.SearchPatterns;

            ProcessComputer();

            Console.WriteLine("Drive Scan Complete");
        }

        private static AppConfig GetAppConfig()
        {
            AppConfig appConfig;

            try
            {
                var builder = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("config.json", optional: false);

                IConfiguration config = builder.Build();

                appConfig = config.GetSection("AppConfig").Get<AppConfig>();
            }
            catch (Exception ex)
            {
                throw new Exception($"There was an issue reading the config.json file. {ex.Message}");
            }
           
            return appConfig;
        }

        private static void ProcessComputer()
        {
            try
            {
                Console.WriteLine($"Processing computer {Environment.MachineName}");

                DriveInfo[] allDrives = DriveInfo.GetDrives();

                foreach (DriveInfo d in allDrives)
                {
                    if (d.IsReady)
                    {
                        ProcessDrive(d);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }
        }

        private static void ProcessDrive(DriveInfo driveInfo)
        {
            try
            {
                Console.WriteLine($"Processing drive {driveInfo.Name}");

                var searchPattern = "*.txt";
                ProcessFiles(driveInfo.RootDirectory.FullName, searchPattern);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }
        }

        public static void ProcessFiles(string path, string searchPattern)
        {
            List<string> folders = new List<string>() { path };
            int folCount = 1;

            for (int i = 0; i < folCount; i++)
            {
                try
                {
                    foreach (var newDir in Directory.EnumerateDirectories(folders[i], "*", SearchOption.TopDirectoryOnly))
                    {
                        folders.Add(newDir);
                        folCount++;
                        try
                        {

                            foreach (var file in Directory.EnumerateFiles(newDir, searchPattern))
                            {
                                ProcessFile(file);
                            }
                        }
                        catch (UnauthorizedAccessException)
                        {
                            // Failed to read a File, skipping it.
                        }
                    }
                }
                catch (UnauthorizedAccessException)
                {
                    // Failed to read a Folder, skipping it.
                    continue;
                }
            }
        }

        private static void ProcessFile(string file)
        {
            try
            {
                Console.WriteLine($"Processing file {file}");

                foreach(var line in File.ReadLines(file))
                {
                    ProcessLine(line);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }
        }

        private static void ProcessLine(string line)
        {
            foreach(var searchPattern in SearchPatterns)
            {
                var match = searchPattern.Regex.Match(line);

                if (match.Success)
                {
                    Console.WriteLine($"Found {searchPattern.Name} Match");
                }
            }
        }
    }
}