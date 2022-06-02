using System;
using NUnit.Framework;
using RegexGenerator.Services;
using Decimal = RegexGenerator.Models.Decimal;

namespace RegexGeneratorTests.Services
{
    [TestFixture]
    internal class DecimalRangeCalculatorTests
    {
        private DecimalRangeCalculator _rangeCalculator;

        [SetUp]
        public void Setup()
        {
            _rangeCalculator = new DecimalRangeCalculator();
        }

        [Test]
        public void Calculates_Decimal_Ranges()
        {
            var min = new Decimal(264, 2);
            var max = new Decimal(734, 0);

            var test = _rangeCalculator.GetRanges(min, max);
            
            foreach (var range in test)
            {
                Console.Write("(");
                Console.Write(range.Min);
                Console.Write(", ");
                Console.Write(range.Max);
                Console.Write(") ");
            }
        }
    }
}
