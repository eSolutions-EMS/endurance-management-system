namespace Not.Blazor.TM.Models;

public class TabModel
{
    public TabModel(Guid id, string header)
    {
        Id = id;
        Header = header;
    }

    public Guid Id { get; set; }
    public string Header { get; set; }
}
