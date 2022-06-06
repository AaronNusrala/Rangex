using System.IO;
using System.Text.Json;

namespace RegexGeneratorTests.TestCases;

public static class TestCaseUtility
{
    public static TTestCase? GetTestCases<TTestCase>(string fileName)
    {
        var file = File.OpenRead($"TestCases/{fileName}.json");
        return JsonSerializer.Deserialize<TTestCase>(file);
    }
}