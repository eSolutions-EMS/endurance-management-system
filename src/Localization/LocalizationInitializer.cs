using EnduranceJudge.Core.Services;
using EnduranceJudge.Localization.Services;
using EnduranceJudge.Localization.Translations;
using System;

namespace EnduranceJudge.Localization;

public class LocalizationInitializer : IInitializer
{
    private readonly IStringsReader stringsReader;
    private readonly IStringsPopulator stringsPopulator;
    public LocalizationInitializer(IStringsReader stringsReader, IStringsPopulator stringsPopulator)
    {
        this.stringsReader = stringsReader;
        this.stringsPopulator = stringsPopulator;
    }

    public int RunningOrder { get; }

    public void Run(IServiceProvider serviceProvider)
    {
        var values = this.stringsReader.Read();
        this.stringsPopulator.Populate(typeof(Messages), values);
        this.stringsPopulator.Populate(typeof(Messages.DomainValidation), values);
    }
}
