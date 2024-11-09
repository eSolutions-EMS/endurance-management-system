namespace Not.Analyzers.Dummies;

public class CodeStyleThisDummy : List<int>
{
    int _field;
    
    void Method() {
        // invalid
        _field = 1;  
        this.NormalMethod();  
        
        // valid
        this.ToList();
        UseThisArgument(this);
    }

    void ExtensionMethod()
    {
        throw new NotImplementedException();
    }

    void NormalMethod()
    {
        throw new NotImplementedException();
    }

    void UseThisArgument(object obj)
    {
    }
}
