using System.Text.RegularExpressions;

namespace Galactic.DriveScanner.App.Models
{
    public class SearchPattern
    {
        private string _pattern = "";
        private Regex _regex;

        public SearchPattern()
        {
            Name = "";
            Pattern = "";
        }

        public string Pattern 
        {
            get
            {
                return _pattern;
            }
            set
            {
                _pattern = value;
                _regex = new Regex(value);
            }
        }

        public string Name { get; set; }

        public int MatchScore { get; set; }

        public bool MatchFound(string text)
        {
            var match = _regex.Match(text);

            return match.Success;
        }
    }
}
