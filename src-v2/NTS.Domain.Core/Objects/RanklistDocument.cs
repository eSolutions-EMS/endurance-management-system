namespace NTS.Domain.Core.Objects;

public record RanklistDocument : Document
{
    public RanklistDocument(DocumentHeader header, Ranklist ranklist) : base(header)
    {
        Ranklist = ranklist;
    }

    public Ranklist Ranklist { get; private set; }
}
