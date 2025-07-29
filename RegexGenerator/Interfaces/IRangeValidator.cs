namespace RegexGenerator.Interfaces;

public interface IRangeValidator<in TNumeric>
{
    (bool Result, string Message) ValidateRange(TNumeric min, TNumeric max);
}