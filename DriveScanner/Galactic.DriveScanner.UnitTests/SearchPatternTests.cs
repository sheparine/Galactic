using Galactic.DriveScanner.App.Models;
using NUnit.Framework;

namespace Galactic.DriveScanner.UnitTests
{
    public class Tests
    {
        private SearchPattern _creditCardSearchPattern = new SearchPattern();

        private SearchPattern _socialSecuritySearchPattern1 = new SearchPattern();

        private SearchPattern _socialSecuritySearchPattern2 = new SearchPattern();

        [SetUp]
        public void Setup()
        {
            _creditCardSearchPattern = new SearchPattern
            {
                Pattern = "\\b\\d{4} \\d{4} \\d{4} \\d{4}\\b"
            };

            _socialSecuritySearchPattern1 = new SearchPattern
            {
                Pattern = "\\b\\d{3}-\\d{2}-\\d{4}\\b"
            };

            _socialSecuritySearchPattern2 = new SearchPattern
            {
                Pattern = ".*(social security)+.*(\\d{3}-\\d{2}-\\d{4})+|.*(\\d{3}-\\d{2}-\\d{4})+.*(social security)+.*"
            };
        }

        [Test]
        public void TestCreditCardSearchPattern()
        {
            Assert.True(_creditCardSearchPattern.MatchFound("4444 4444 4444 4444"));
            Assert.False(_creditCardSearchPattern.MatchFound("44444444 4444 4444"));
            Assert.False(_creditCardSearchPattern.MatchFound("444444444444 4444"));
            Assert.False(_creditCardSearchPattern.MatchFound("4444444444444444"));
            Assert.False(_creditCardSearchPattern.MatchFound("44444444 44444444"));
            Assert.False(_creditCardSearchPattern.MatchFound("4444 444444444444"));
            Assert.False(_creditCardSearchPattern.MatchFound("444444i4 4444 4444"));
        }

        [Test]
        public void TestSocialSecuritySearchPattern1()
        {
            Assert.True(_socialSecuritySearchPattern1.MatchFound("444-44-4444"));
            Assert.False(_socialSecuritySearchPattern1.MatchFound("44444-4444"));
            Assert.False(_socialSecuritySearchPattern1.MatchFound("444-444444"));
            Assert.False(_socialSecuritySearchPattern1.MatchFound("444444444"));
            Assert.False(_socialSecuritySearchPattern1.MatchFound("444-i4-4444"));
        }

        [Test]
        public void TestSocialSecuritySearchPattern2()
        {
            Assert.False(_socialSecuritySearchPattern2.MatchFound("444-44-4444"));
            Assert.True(_socialSecuritySearchPattern1.MatchFound("444-44-4444 Social Security"));
            Assert.True(_socialSecuritySearchPattern1.MatchFound("Social Security 444-44-4444"));
        }
    }
}