using EnduranceJudge.Core.Services;
using EnduranceJudge.Localization.Services;
using EnduranceJudge.Localization.Translations;
using System;

namespace EnduranceJudge.Localization;

public class LocalizationInitializer : IInitializer
{
    private readonly ITranslationsReader translationsReader;
    private readonly IPopulator populator;
    public LocalizationInitializer(ITranslationsReader translationsReader, IPopulator populator)
    {
        this.translationsReader = translationsReader;
        this.populator = populator;
    }

    public int RunningOrder { get; }

    public void Run(IServiceProvider serviceProvider)
    {
        var values = this.translationsReader.Read();
        this.populator.Populate(typeof(Messages), values);
        this.populator.Populate(typeof(Messages.DomainValidation), values);
    }
}
