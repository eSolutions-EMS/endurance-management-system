namespace EnduranceJudge.Domain.Aggregates.Common.Countries
{
    public interface ICountryState
    {
        string IsoCode { get; }

        string Name { get; }
    }
}
