using RegexGenerator.Enumerations;
using RegexGenerator.Models;

namespace RegexGenerator;

public static class NumberRangeRegexGenerator
{
    public static string GenerateNumberRangeRegex(string min, string max, RegexGeneratorOptions options = null)
    {
        var inputMode = options?.InputMode;
        var outputMode = options?.OutputMode;

        if (inputMode == GenerationMode.Detect)
        {
            inputMode = min.Contains('.') || max.Contains('.') 
                ? GenerationMode.Decimal
                : GenerationMode.Integer;
        }

        if (outputMode == GenerationMode.Detect)
        {
            outputMode = inputMode;
        }
    }
}