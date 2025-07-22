namespace RegexGenerator.Models.NumberSystems;

public abstract class NumberSystem
{
    protected NumberSystem(char[,] characters, int radix)
    {
        Characters = characters;
        Radix = radix;
    }

    protected char[,] Characters { get; }
    
    protected int Radix { get; }

    public int Parse(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            throw new ArgumentException("Input cannot be null or empty.", nameof(input));
        }

        var result = 0;
        
        for (var i = 0; i < input.Length; i++)
        {
            var c = input[i];
            for (var row = 0; row < Characters.GetLength(0); row++)
            {
                for (int col = 0; col < Characters.GetLength(1); col++)
                {
                    if (Characters[row, col] == c)
                    {
                        digitValue = row * Characters.GetLength(1) + col;
                        break;
                    }
                }
                if (digitValue != -1) break;
            }
            if (digitValue == -1)
                throw new ArgumentException($"Invalid character '{c}' in input.");
            result = result * Radix + digitValue;
        }
        return result;
    }
}