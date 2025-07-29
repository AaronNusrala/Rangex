using NUnit.Framework;
using RegexGenerator.Models.NumberSystems;
using RegexGenerator.Utilities;

namespace RegexGeneratorTests.Tests.Utilities;

public class MathUtilitiesTests
{
  [TestCase(2, 2, 4)]
  [TestCase(2, 3, 8)]
  [TestCase(0, 0, 1)]
  [TestCase(1, 0, 1)]
  [TestCase(1, 1, 1)]
  [TestCase(0, 1, 0)]
  [TestCase(0, 10, 0)]
  public void TestPower(int @base, int exponent, int expected)
  {
    var result = @base.Pow(exponent);
    Assert.That(result, Is.EqualTo(expected));
  }

  [TestCase(2222, 0, 2220)]
  [TestCase(2222, 1, 2200)]
  [TestCase(2222, 2, 2000)]
  [TestCase(2222, 3, 0)]
  public void TestZeros(int input, int index, int expected)
  {
    var result = input.Zeros<int, DecimalNumbers>(index);
    Assert.That(result, Is.EqualTo(expected));
  }
  
  [TestCase(2222, 0, 2229)]
  [TestCase(2222, 1, 2299)]
  [TestCase(2222, 2, 2999)]
  [TestCase(2222, 3, 9999)]
  public void TestNines(int input, int index, int expected)
  {
    var result = input.Nines<int, DecimalNumbers>(index);
    Assert.That(result, Is.EqualTo(expected));
  }
  
  [TestCase(1234, 0, 4)]
  [TestCase(1234, 1, 3)]
  [TestCase(1234, 2, 2)]
  [TestCase(1234, 3, 1)]
  public void TestDigitAt(int input, int index, int expected)
  {
    var result = input.DigitAt<int, DecimalNumbers>(index);
    Assert.That(result, Is.EqualTo(expected));
  }
}