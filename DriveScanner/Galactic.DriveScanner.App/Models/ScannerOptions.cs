
namespace Galactic.DriveScanner.App.Models
{
    public class ScannerOptions
    {
        public ScannerOptions()
        {
            SearchPatterns = new List<SearchPattern>();
            OutputFile = "output.txt";
            FileExtensions = new List<string>();
        }

        public List<SearchPattern> SearchPatterns { get; set; }

        public string OutputFile { get; set; }

        public List<string> FileExtensions { get; set; }
    }
}
