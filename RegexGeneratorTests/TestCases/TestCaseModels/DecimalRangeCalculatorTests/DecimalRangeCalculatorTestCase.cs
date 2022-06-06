namespace RegexGeneratorTests.TestCases.TestCaseModels.DecimalRangeCalculatorTests;

public class DecimalRangeCalculatorTestCase
{
    public decimal Min { get; set; }
            
    public decimal Max { get; set; }
    
    public ExpectedDecimalRange[] ExpectedRanges { get; set; }

    public override string ToString() => $"{Min} => {Max}";
}