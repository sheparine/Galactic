
namespace Galactic.DriveScanner.App.Models
{
    public class ScannerOptions
    {
        public ScannerOptions()
        {
            SearchPatterns = new List<SearchPattern>();
            OutputFile = "output.txt";
        }

        public List<SearchPattern> SearchPatterns { get; set; }

        public string OutputFile { get; set; }
    }
}
