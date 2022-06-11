using NUnit.Framework;
using RegexGenerator;

namespace RegexGeneratorTests.Tests
{
    [TestFixture]
    public class NumberRangeRegexGeneratorTests
    {
        private NumberRangeRegexGenerator? _regexGenerator;

        [SetUp]
        public void Setup() => _regexGenerator = new NumberRangeRegexGenerator();
        
        [TestCase(1, 1, "^(1)$")]
        [TestCase(1, 2, "^([1-2])$")]
        [TestCase(1, 10, "^([1-9]|10)$")]
        [TestCase(1, 100, "^([1-9]|[1-9][0-9]|100)$")]
        [TestCase(23, 8483, "^(2[3-9]|[3-9][0-9]|[1-9][0-9][0-9]|[1-7][0-9][0-9][0-9]|8[0-3][0-9][0-9]|84[0-7][0-9]|848[0-3])$")]
        public void Generates_A_Valid_Regex(int min, int max, string expectedRegex)
        {
            var generatedRegex = _regexGenerator?.GenerateRegex(min, max);
            Assert.AreEqual(expectedRegex, generatedRegex);
        }
    }
}
