using NTS.TagRfidControllers.Writer;
using NTS.TagRfidControllers.Reader;
class RfidControllers
{
    static async Task Main(string[] args)
    {
        if(args.Length == 1)
        {
            var argument = Int32.Parse(args[0]);
            await WriteTag(argument);
            return;
        }
        else
        {
            await ReadTags();
        }
    }

    static async Task WriteTag(int combinationNumber)
    {
        var writer = new Writer();
        await Task.Run(() => writer.Write(combinationNumber));
    }

    static async Task ReadTags()
    {
        var reader = new Reader();
        await Task.Run(() => reader.ReadTags());
    }
}
