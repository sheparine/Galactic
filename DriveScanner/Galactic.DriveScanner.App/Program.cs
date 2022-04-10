using Galactic.DriveScanner.App.Models;
using Microsoft.Extensions.Configuration;

namespace Galactic.DriveScanner.App
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Starting Drive Scanner");

            var options = GetOptions();

            var scanner = new Scanner(options);

            scanner.ProcessComputer();

            Console.WriteLine("Drive Scan Complete");
        }

        private static ScannerOptions GetOptions()
        {
            ScannerOptions options;

            try
            {
                var builder = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("config.json", optional: false);

                IConfiguration config = builder.Build();

                options = config.GetSection("ScannerOptions").Get<ScannerOptions>();
            }
            catch (Exception ex)
            {
                throw new Exception($"There was an issue reading the config.json file. {ex.Message}");
            }
           
            return options;
        }
    }
}