
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

        public async Task ProcessComputerAsync()
        {
            try
            {
                Console.WriteLine($"Processing computer {Environment.MachineName}");

                var allDrives = DriveInfo.GetDrives();

                foreach (var d in allDrives)
                {
                    if (d.IsReady)
                    {
                        await ProcessDriveAsync(d);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }
        }

        protected async Task ProcessDriveAsync(DriveInfo driveInfo)
        {
            Console.WriteLine($"Processing drive {driveInfo.Name}");

            var enumerationOptions = new EnumerationOptions 
            { 
                IgnoreInaccessible = true,
                RecurseSubdirectories = true
            };

            var fileNames = Directory.EnumerateFiles(driveInfo.RootDirectory.FullName, "*", enumerationOptions);

            foreach (var fileName in fileNames)
            {
                try
                {
                    var fi = new FileInfo(fileName);
                    if (_options.FileExtensions.Contains(fi.Extension))
                    {
                        await ProcessFileAsync(fileName);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.InnerException);
                }
            }
        }

        protected async Task ProcessFileAsync(string filePath)
        {
            try
            {
                Console.WriteLine($"Processing file {filePath}");

                foreach (var line in File.ReadLines(filePath))
                {
                    await ProcessLineAsync(filePath, line);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }
        }

        protected async Task ProcessLineAsync(string filePath, string line)
        {
            foreach (var searchPattern in _options.SearchPatterns)
            {
                if (searchPattern.MatchFound(line))
                {
                    var message = $"{filePath} - {searchPattern.Name} (Match Score {searchPattern.MatchScore})";
                    Console.WriteLine(message);
                    using StreamWriter file = new(_options.OutputFile, append: true);
                    await file.WriteLineAsync(message);
                }
            }
        }
    }
}