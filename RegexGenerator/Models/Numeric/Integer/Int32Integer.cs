using RegexGenerator.Interfaces;
using RegexGenerator.Utilities;

namespace RegexGenerator.Models.Integer;

internal class Int32Integer : IInteger<int>
{
    private int _value;

    public Int32Integer(int value)
    {
        _value = value;
    }

    //Nines(1111, 2) -> 1999
    public IInteger Nines(int value, int index)
    {
        var t = 10.Pow(index + 1);
        return new Int32Integer(value - value % t + t  - 1);
    }

    //Zeros(1111, 2) -> 1000
    public IInteger Zeros(int value, int index)
    {
        var t = 10.Pow(index + 1);
        return new Int32Integer(value - value % t);
    }
    
    //DigitAt(123, 1) -> 2
    public int DigitAt(int value, int index) =>  value / 10.Pow(index) % 10;
    public bool IsNegative { get; }
    
    static void IInteger.operator +(IInteger left, int right)
    {
        throw new NotImplementedException();
    }
}