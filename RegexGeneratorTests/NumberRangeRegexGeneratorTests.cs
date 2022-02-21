using NUnit.Framework;
using RegexGenerator;
using RegexGenerator.Services;

namespace RegexGeneratorTests
{
    [TestFixture]
    public class NumberRangeRegexGeneratorTests
    {
        private NumberRangeRegexGenerator _regexGenerator;

        [SetUp]
        public void Setup()
        {
            var rangeCalculator = new RangeCalculator();
            var regexBuilder = new RegexBuilder();
            _regexGenerator = new NumberRangeRegexGenerator(rangeCalculator, regexBuilder);
        }

        [TestCase(1, 1, "^(1)$")]
        [TestCase(1, 2, "^([1-2])$")]
        [TestCase(1, 10, "^([1-9]|10)$")]
        [TestCase(1, 100, "^([1-9]|[1-9][0-9]|100)$")]

        public void Generates_A_Valid_Regex(int min, int max, string expectedRegex)
        {
            var generatedRegex = _regexGenerator.GenerateRegex(min, max);
            Assert.AreEqual(expectedRegex, generatedRegex);
        }
    }
}
