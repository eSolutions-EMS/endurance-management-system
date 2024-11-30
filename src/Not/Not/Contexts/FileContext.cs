using Not.Injection.Config;

namespace Not.Contexts;

public class FileContext : NConfig, IFileContext // TODO: move to Filesystem
{
    string _path = default!;

    protected override string[] RequiredFields => [nameof(Path)];

    public string Path
    {
        get => _path;
        set => _path = value ?? throw new ArgumentException($"{nameof(IFileContext)}.{nameof(Path)} cannot be null");
    }
}

public interface IFileContext : INConfig
{
    string Path { get; set; }
}
