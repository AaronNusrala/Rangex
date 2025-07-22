namespace RegexGenerator.Interfaces;

internal interface IInteger
{
    bool IsNegative { get; }
    
    int DigitAt(int index);
    
    IInteger Nines(int index);
    
    IInteger Zeros(int index);
}

internal interface IInteger<TUnderlying> : IInteger
{
}