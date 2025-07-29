using NUnit.Framework;
using RegexGenerator.Services;

namespace RegexGeneratorTests.Tests.Services;

[TestFixture]
public class RegexBuilderTests
{
    private RegexBuilder _regexBuilder;

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
    
    [Test]
    public void Test_Or()
    {
        _regexBuilder.MatchLiteral("A").Or().MatchLiteral("B");
        var regex = _regexBuilder.ToRegex();
        Assert.That(regex, Is.EqualTo("A|B"));
    }
    
    [Test]
    public void Match_Literal()
    {
        _regexBuilder.MatchLiteral("A");
        var regex = _regexBuilder.ToRegex();
        Assert.That(regex, Is.EqualTo("A"));
    }
}