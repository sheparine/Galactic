using Galactic.DriveScanner.App.Models;

namespace Galactic.DriveScanner.App
{
    public class AppConfig
    {
        public AppConfig()
        {
            SearchPatterns = new List<SearchPattern>();
        }

        public List<SearchPattern> SearchPatterns { get; set; }
    }
}
