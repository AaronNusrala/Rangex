using NUnit.Framework;
using RegexGenerator.Services;

namespace RegexGeneratorTests.Tests.Services
{
    [TestFixture]
    public class RegexBuilderTests
    {
        private RegexBuilder _regexBuilder = default;

        [SetUp]
        public void Setup()
        {
            _regexBuilder = new RegexBuilder();
        }

        [Test]
        public void Appends_Numeric_Character_Group()
        {
            _regexBuilder.CharacterClassRange('1', '2');
            var regex = _regexBuilder.ToRegex();
            Assert.That(regex, Is.EqualTo("[1-2]"));
        }
    }
}
