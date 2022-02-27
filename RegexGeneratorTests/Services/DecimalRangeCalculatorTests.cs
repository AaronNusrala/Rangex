using System;
using System.Linq;
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
            var min = new Decimal
            {
                Value = 28,
                LeadingZeros = 4
            };

            var max = new Decimal
            {
                LeadingZeros = 0,
                Value = 738
            };

            var test = _rangeCalculator
                .GetRanges(min, max)
                .OrderByDescending(r => r.Min.LeadingZeros)
                .ThenBy(r => r.Min.Value);

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
