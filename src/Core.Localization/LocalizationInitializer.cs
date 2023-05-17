using Core.Localization.Services;
using Core.Services;
using System;

namespace Core.Localization;

public class LocalizationInitializer : IInitializer
{
    private readonly IStringsReader stringsReader;
    private readonly IPlaceholderProcessor placeholderProcessor;
    private readonly IStringsPopulator stringsPopulator;
    public LocalizationInitializer(
        IStringsReader stringsReader,
        IPlaceholderProcessor placeholderProcessor,
        IStringsPopulator stringsPopulator)
    {
        this.stringsReader = stringsReader;
        this.placeholderProcessor = placeholderProcessor;
        this.stringsPopulator = stringsPopulator;
    }

    public int RunningOrder => 10;

    public void Run()
    {
        var values = this.stringsReader.Read();
        var processed = this.placeholderProcessor.Process(values);
        this.stringsPopulator.Populate(typeof(Strings), processed);
    }
}
