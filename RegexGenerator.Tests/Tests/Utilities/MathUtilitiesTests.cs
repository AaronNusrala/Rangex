using NUnit.Framework;
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
    var result = input.Zeros(index);
    Assert.That(result, Is.EqualTo(expected));
  }
}