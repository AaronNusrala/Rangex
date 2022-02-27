using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using RegexGenerator.Models;
using RegexGenerator.Services;

namespace RegexGeneratorTests.Services
{
    [TestFixture]
    public class RangeCalculatorTests
    {
        private RangeCalculator _rangeCalculator;

        [SetUp]
        public void Setup()
        {
            _rangeCalculator = new RangeCalculator();
        }

        [TestCaseSource(nameof(TestCases))]
        public void Generates_Ranges(int min, int max, (int Min, int Max)[] expectedRanges)
        {
            var actualRanges = _rangeCalculator.CalculateRanges(min, max).ToList();
            var actualRangeList = actualRanges.ToList();

            if (expectedRanges.Length != actualRangeList.Count)
            {
                var message = CreateMessage(expectedRanges, actualRangeList);
                throw new AssertionException(message);
            }

            foreach (var (expectedMin, expectedMax) in expectedRanges)
            {
                var actualRange = actualRangeList.FirstOrDefault(r => r.Min == expectedMin && r.Max == expectedMax);

                if (actualRange == null)
                {
                    var message = CreateMessage(expectedRanges, actualRanges);
                    throw new AssertionException(message);
                }

                actualRangeList.Remove(actualRange);
            }
        }

        private static string CreateMessage(IEnumerable<(int Min, int Max)> expectedRanges, IEnumerable<RegexRange> actualRanges)
        {
            var sb = new StringBuilder();
            sb.Append("Expected: ");

            foreach (var (min, max) in expectedRanges.OrderBy(r => r.Min))
            {
                sb.Append("(").Append(min).Append(", ").Append(max).Append(") ");
            }

            sb.AppendLine();
            sb.Append("Got:      ");

            foreach (var (actualMin, actualMax) in actualRanges.OrderBy(r => r.Min))
            {
                sb.Append("(").Append(actualMin).Append(", ").Append(actualMax).Append(") ");
            }

            return sb.ToString();
        }

        private static object[] TestCases =
        {
            new object[] { 0, 0, new []{ (0, 0) } },
            new object[] { 0, 1, new []{ (0, 1) } },
            new object[] { 1, 1, new []{ (1, 1) } },
            new object[] { 1, 2, new []{ (1, 2) } },
            new object[] { 1, 10, new []{ (1, 9), (10, 10) } },
            new object[] { 1, 100, new []{ (1, 9), (10, 99), (100, 100) } },
            new object[] { 1, 101, new []{ (1, 9), (10, 99), (100, 101) } },
            new object[] { 3, 4382, new []{ (3, 9), (10, 99), (100, 999), (1000, 3999), (4000, 4299), (4300, 4379), (4380, 4382) } }
        };
    }
}