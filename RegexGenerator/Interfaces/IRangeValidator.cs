namespace RegexGenerator.Interfaces;

public interface IRangeValidator<in TNumeric>
{
    void ValidateRange(TNumeric min, TNumeric max);
}