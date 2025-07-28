namespace RegexGenerator.Models.NumberSystems;

public abstract class CharacterSetNumberSystem
{
    private static int Radix(char[] characters) => characters.Length;
    
    public static int Lookup(char character, char[] characters)
    {
        var radix = Radix(characters);
        
        for (var i = 0; i < radix; i++)
        {
            if (characters[i] == character)
            {
                return i;
            }
        }

        throw new ArgumentOutOfRangeException(nameof(character), character, null);
    }
    
    protected static char Lookup(int value, char[] characters)
    {
        if (value < 0 || value >= characters.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(value), value,
                $"Value must be between '0' and '{characters.Length - 1}'.");
        }

        return characters[value];
    }
}
