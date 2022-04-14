
namespace Galactic.DriveScanner.App.Models
{
    public class Scanner
    {
        private readonly ScannerOptions _options;

        public Scanner(ScannerOptions options)
        {
            _options = options;
            File.Delete(_options.OutputFile);
        }

        public void ProcessComputer()
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

        protected void ProcessDrive(DriveInfo driveInfo)
        {
            Console.WriteLine($"Processing drive {driveInfo.Name}");

            var enumerationOptions = new EnumerationOptions 
            { 
                IgnoreInaccessible = true,
                RecurseSubdirectories = true
            };

            var files = Directory.EnumerateFiles(driveInfo.RootDirectory.FullName, "*", enumerationOptions);

            foreach (var file in files)
            {
                try
                {
                    ProcessFile(file);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.InnerException);
                }
            }
        }

        protected void ProcessFile(string filePath)
        {
            try
            {
                Console.WriteLine($"Processing file {filePath}");

                foreach (var line in File.ReadLines(filePath))
                {
                    ProcessLine(filePath, line);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }
        }

        protected void ProcessLine(string filePath, string line)
        {
            foreach (var searchPattern in _options.SearchPatterns)
            {
                if (searchPattern.MatchFound(line))
                {
                    var message = $"{filePath} - ({ searchPattern.Name})";
                    Console.WriteLine(message);
                    using StreamWriter file = new(_options.OutputFile, append: true);
                    file.WriteLineAsync(message);
                }
            }
        }
    }
}