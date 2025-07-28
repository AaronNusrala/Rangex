namespace RegexGenerator.Models.NumberSystems;

public abstract class ConsecutiveAsciiCharacterNumberSystem
{ 
    protected static int Lookup(char character, char start, char end)
    {
        if (character < start || character > end)
        {
            throw new ArgumentOutOfRangeException(nameof(character), character,
                $"Character must be between '{start}' and '{end}'.");
        }

        return character - start;
    }

    protected static char Lookup(int value, char start, char end)
    {
        var characterCode = start + value;
        
        if (characterCode > end)
        {
            throw new ArgumentOutOfRangeException(nameof(value), value,
                $"Value must be between '0' and '{end - start}'.");
        }

        return (char) characterCode;
    }
}