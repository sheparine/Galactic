
using System.Text.RegularExpressions;

namespace Galactic.DriveScanner.App.Models
{
    public class SearchPattern
    {
        public SearchPattern()
        {
            Name = "";
            Pattern = "";
        }

        private string _pattern = "";

        public string Pattern 
        {
            get
            {
                return _pattern;
            }
            set
            {
                _pattern = value;
                Regex = new Regex(value);
            }
        }

        public string Name { get; set; }

        public Regex Regex { get; private set; }

        public bool MatchFound(string text)
        {
            var match = Regex.Match(text);

            return match.Success;
        }
    }
}
