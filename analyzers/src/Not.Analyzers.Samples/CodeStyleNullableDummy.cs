namespace NTS.Judge.Blazor.CodeStyle;

public class CodeStyleNullableDummy : Abstraction, IAbstraction
{
    string _nonNullShouldHaveValue;
    string _nonNullableCannotBeNull = null;
}

public abstract class Abstraction { }

public interface IAbstraction { }
