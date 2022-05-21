using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace EnduranceJudge.Tools.Console.PrintCountries
{
    public static class CountryService
    {
        public static void Print()
        {
            var relativePath = "PrintCountries/input.json";
            var filePath = GetFullPath(relativePath);
            var contents = File.ReadAllText(filePath);
            var settings = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var models = JsonSerializer.Deserialize<List<CountryModel>>(contents, settings);

            var sb = new StringBuilder();
            var count = 0;
            foreach (var country in models)
            {
                ++count;
                var line = $"new (\"{country.Code}\", \"{country.Name}\", {count}),";
                sb.AppendLine(line);
            }

            var outputFilePath = GetFullPath("PrintCountries/output.txt");
            var output = sb.ToString();
            File.WriteAllText(outputFilePath, output);
        }

        private static string GetFullPath(string relativePath)
        {
            var solutionRootPath = $"../../../{relativePath}";
            return Path.Combine(
                Directory.GetCurrentDirectory(),
                solutionRootPath);
        }
    }
}
