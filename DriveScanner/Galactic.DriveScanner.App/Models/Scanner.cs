
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

        protected void ProcessFiles(string path, string searchPattern)
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
                var match = searchPattern.Regex.Match(line);

                if (match.Success)
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