using System.Text;
using RegexGenerator.Models.Regex;

namespace RegexGenerator.Services
{
    public interface IRegexBuilder
    {
        IRegexBuilder MatchLiteralCharacter(char character);

        IRegexBuilder MatchLiteral(IEnumerable<char> characters);

        IRegexBuilder BeginString();

        IRegexBuilder EndString();

        IRegexBuilder Group();

        IRegexBuilder EndGroup();

        IRegexBuilder CharacterClassRange(char min, char max);

        IRegexBuilder Or();

        IRegexBuilder Repeat(int count);
        
        IRegexBuilder MatchIntegerRange(SignedRegexIntegerRange range);
        
        IRegexBuilder CharacterClassRange(RegexCharacterClass characterClass);

        string ToRegex();
    }

    internal class RegexBuilder : IRegexBuilder
    {
        private readonly StringBuilder _stringBuilder = new();

        public IRegexBuilder MatchLiteralCharacter(char character)
        {
            _stringBuilder.Append(character);
            return this;
        }

        public IRegexBuilder MatchLiteral(IEnumerable<char> characters)
        {
            foreach(var character in characters)
            {
                _stringBuilder.Append(character);
            }

            return this;
        }

        public IRegexBuilder BeginString()
        {
            _stringBuilder.Append('^');
            return this;
        }

        public IRegexBuilder EndString()
        {
            _stringBuilder.Append('$');
            return this;
        }

        public IRegexBuilder Group()
        {
            _stringBuilder.Append('(');
            return this;
        }

        public IRegexBuilder EndGroup()
        {
            _stringBuilder.Append(')');
            return this;
        }

        public IRegexBuilder CharacterClassRange(char min, char max)
        {
            _stringBuilder.Append('[').Append(min).Append('-').Append(max).Append(']');
            return this;
        }

        public IRegexBuilder Repeat(int count)
        {
            _stringBuilder.Append('{').Append(count).Append('}');
            return this;
        }

        //TODO get rid of this or make it handle signs.
        public IRegexBuilder MatchIntegerRange(SignedRegexIntegerRange range)
        {
            MatchLiteral(range.Prefix);

            foreach (var characterClass in range.Suffix)
            {
                CharacterClassRange(characterClass);
            }

            return this;
        }

        public IRegexBuilder CharacterClassRange(RegexCharacterClass characterClass)
        {
            return CharacterClassRange(characterClass.Start, characterClass.End);
        }

        public IRegexBuilder Or()
        {
            _stringBuilder.Append('|');
            return this;
        }
        
        public string ToRegex() => _stringBuilder.ToString();
    }
}
