using EnduranceJudge.Core.Services;
using EnduranceJudge.Localization.Services;
using EnduranceJudge.Localization.Translations;
using System;

namespace EnduranceJudge.Localization;

public class LocalizationInitializer : IInitializer
{
    private readonly ITranslationsReader translationsReader;
    private readonly IStringsPopulator stringsPopulator;
    public LocalizationInitializer(ITranslationsReader translationsReader, IStringsPopulator stringsPopulator)
    {
        this.translationsReader = translationsReader;
        this.stringsPopulator = stringsPopulator;
    }

    public int RunningOrder { get; }

    public void Run(IServiceProvider serviceProvider)
    {
        var values = this.translationsReader.Read();
        this.stringsPopulator.Populate(typeof(Messages), values);
        this.stringsPopulator.Populate(typeof(Messages.DomainValidation), values);
    }
}
