using System.Diagnostics.CodeAnalysis;
using Not.Random;

namespace Not.Domain;

public abstract class DomainEntity : IEquatable<DomainEntity>, IIdentifiable
{
    public static bool operator ==(DomainEntity? left, DomainEntity? right)
    {
        return left?.IsEqual(right) ?? right is null;
    }

    public static bool operator !=(DomainEntity? left, DomainEntity? right)
    {
        return !(left == right);
    }

    protected DomainEntity(int id)
    {
        Id = id;
    }

    // TODO: use DomainObject for ID, do private set
    public int Id { get; }

    protected string Combine(params object?[] values)
    {
        var sections = values.Where(x => x != null);
        return string.Join(" | ", sections);
    }

    protected static int GenerateId()
    {
        return RandomHelper.GenerateUniqueInteger();
    }

    protected static T NotDefault<T>(string field, T value)
        where T : struct
    {
        if (value.Equals(default))
        {
            throw GetRequiredException(field);
        }
        return value;
    }

    protected static T Required<T>(string field, T? value)
        where T : struct
    {
        return value ?? throw GetRequiredException(field);
    }

    [return: NotNull]
    protected static T Required<T>(string field, T? instance)
        where T : class
    {
        return instance ?? throw GetRequiredException(field);
    }

    [return: NotNull]
    protected static string Required(string field, string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw GetRequiredException(field);
        }
        return value;
    }

    public bool Equals(DomainEntity? other)
    {
        return IsEqual(other);
    }

    public override bool Equals(object? other)
    {
        return IsEqual(other);
    }

    public override int GetHashCode()
    {
        return Id;
    }

    public override string ToString()
    {
        throw new NotImplementedException(
            $"'{GetType().Name}' has to override ToString() to provide short info"
        );
    }

    bool IsEqual(object? other)
    {
        if (other is null or not DomainEntity)
        {
            return false;
        }
        if (ReferenceEquals(this, other))
        {
            return true;
        }
        return GetHashCode() == other.GetHashCode() && GetType() == other.GetType();
    }

    static DomainException GetRequiredException(string field)
    {
        return new DomainException(field, "Please provide value");
    }
}
