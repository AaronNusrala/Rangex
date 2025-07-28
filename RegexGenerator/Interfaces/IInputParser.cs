namespace RegexGenerator.Interfaces;

internal interface IInputParser
{ 
    IParseResult ParseInput<TNumberSystem>(string min, string max) where TNumberSystem : INumberSystem;
}