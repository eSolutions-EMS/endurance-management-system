using EnduranceJudge.Core.ConventionalServices;
using EnduranceJudge.Core.Services;
using EnduranceJudge.Localization.Static;
using System;
using static EnduranceJudge.Localization.LocalizationConstants;

namespace EnduranceJudge.Localization.Services;

public class TranslationsReader : ITranslationsReader
{
    private const string ERROR_TEMPLATE = "Invalid translation file. {0} For more information: 'https://github.com/HorseSport-achobanov/endurance-judge/blob/develop/README.md#translation'";
    private readonly IFileService fileService;

    public TranslationsReader(IFileService fileService)
    {
        this.fileService = fileService;
    }

    public void Read()
    {
        using var stream = this.fileService.ReadStream($"./{TRANSLATION_FILE_NAME}");
        var line = stream.ReadLine(); // header line
        var lineNumber = 0;
        while (line != null)
        {
            line = stream.ReadLine();
            lineNumber++;
            this.ProcessLine(line, lineNumber);
        }
    }

    private void ProcessLine(string line, int lineNumber)
    {
        var (key, baseline, translation) = this.ParseColumnValues(line, lineNumber);
        this.ValidatePlaceholders(baseline, translation);
        Translation.Values.Add(key, translation);
    }

    private void ValidatePlaceholders(string baseline, string translation)
    {
        foreach (var (_, placeholder) in LocalizationConstants.Placeholders)
        {
            if (baseline.Contains(placeholder) != translation.Contains(placeholder))
            {
                var message = $"Baseline '{baseline}' and it's translation equivalent '{translation}' have not-matching placeholders. ";
                this.Throw(message);
            }
        }
    }

    private (string key, string baseline, string translation) ParseColumnValues(string line, int lineNumber)
    {
        var values = line.Split('|', StringSplitOptions.RemoveEmptyEntries);
        if (values.Length != 3)
        {
            var message = $"Line {lineNumber} has {values.Length} values. Each line should contain EXACTLY 3 values, separated by '|'.";
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

public interface ITranslationsReader : IService
{
    void Read();
}
