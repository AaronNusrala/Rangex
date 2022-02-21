using System.Text;

namespace RegexGenerator.Services
{
    internal interface IRegexBuilder
    {
        IRegexBuilder MatchLiteralCharacter(char character);

        IRegexBuilder BeginString();

        IRegexBuilder EndString();

        IRegexBuilder Group();

        IRegexBuilder EndGroup();

        IRegexBuilder CharacterClassRange(char min, char max);

        IRegexBuilder Or();

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

        public IRegexBuilder BeginString()
        {
            _stringBuilder.Append("^");
            return this;
        }

        public IRegexBuilder EndString()
        {
            _stringBuilder.Append("$");
            return this;
        }

        public IRegexBuilder Group()
        {
            _stringBuilder.Append("(");
            return this;
        }

        public IRegexBuilder EndGroup()
        {
            _stringBuilder.Append(")");
            return this;
        }

        public IRegexBuilder CharacterClassRange(char min, char max)
        {
            _stringBuilder.Append("[").Append(min).Append("-").Append(max).Append("]");
            return this;
        }

        public IRegexBuilder Or()
        {
            _stringBuilder.Append("|");
            return this;
        } 

        public string ToRegex() => _stringBuilder.ToString();
    }
}
