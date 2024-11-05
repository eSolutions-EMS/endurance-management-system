using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Core.ConventionalServices;
using Core.Services;
using static Core.Localization.LocalizationConstants;

namespace Core.Localization.Services;

public class StringsReader : IStringsReader
{
    private const string ERROR_TEMPLATE =
        "Invalid translation file. {0} For more information: 'https://github.com/HorseSport-achobanov/endurance-judge/blob/develop/README.md#translation'";
    private readonly IFileService fileService;

    public StringsReader(IFileService fileService)
    {
        this.fileService = fileService;
    }

    public Dictionary<string, string> Read()
    {
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), STRINGS_FILENAME);
        using var stream = this.fileService.ReadStream(filePath);
        var line = stream.ReadLine(); // header line
        var lineNumber = 0;
        var values = new Dictionary<string, string>();
        while (true)
        {
            line = stream.ReadLine();
            if (line == null)
                break;
            lineNumber++;
            var (key, translation) = this.ProcessLine(line, lineNumber);
            values.Add(key, translation);
        }

        return values;
    }

    private (string key, string translation) ProcessLine(string line, int lineNumber)
    {
        var (key, baseline, translation) = this.ParseColumnValues(line, lineNumber);
        // this.ValidatePlaceholders(baseline, translation);
        return (key, translation);
    }

    // private void ValidatePlaceholders(string baseline, string translation)
    // {
    //     foreach (var (placeholder, _) in LocalizationConstants.PLACEHOLDERS)
    //     {
    //         if (baseline.Contains(placeholder) != translation.Contains(placeholder))
    //         {
    //             var message = $"Baseline '{baseline}' and it's translation equivalent '{translation}' have not-matching placeholders. ";
    //             this.Throw(message);
    //         }
    //     }
    // }

    private (string key, string baseline, string translation) ParseColumnValues(
        string line,
        int lineNumber
    )
    {
        var values = line.Split('|', StringSplitOptions.RemoveEmptyEntries)
            .Select(x => x.Trim())
            .ToList();
        if (values.Count != 3)
        {
            var message =
                $"Line {lineNumber} has {values.Count} values. Each line should contain EXACTLY 3 values, separated by '|'.";
            this.Throw(message);
        }
        var key = values[0];
        var baseline = values[1];
        var translation = values[2];
        const string invalidValue = "'{0}' value is null or empty.";
        if (string.IsNullOrWhiteSpace(key))
        {
            var message = string.Format(invalidValue, nameof(key));
            this.Throw(message);
        }
        if (string.IsNullOrWhiteSpace(baseline))
        {
            var message = string.Format(invalidValue, nameof(baseline));
            this.Throw(message);
        }
        if (string.IsNullOrWhiteSpace(translation))
        {
            var message = string.Format(invalidValue, nameof(translation));
            this.Throw(message);
        }
        return (key, baseline, translation);
    }

    private void Throw(string message)
    {
        var fullMessage = string.Format(ERROR_TEMPLATE, message);
        throw new InvalidOperationException(fullMessage);
    }
}

public interface IStringsReader : ITransientService
{
    Dictionary<string, string> Read();
}
