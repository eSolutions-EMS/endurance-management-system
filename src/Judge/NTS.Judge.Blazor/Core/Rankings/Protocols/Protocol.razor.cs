using NTS.Domain.Core.Objects;

namespace NTS.Judge.Blazor.Core.Rankings.Protocols;

public partial class Protocol
{
    [Parameter, EditorRequired]
    public Ranklist Ranklist { get; set; } = default!;
}
