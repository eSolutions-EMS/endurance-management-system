using System;

namespace Not.Analyzers.Samples;

public class CodeStyleSpaceSeparationDummy
{
    public CodeStyleSpaceSeparationDummy()
    {
        var a = 5;

        if (a == 10) { }
        if (a == 10) { }
        if (a == 10) { }

        if (true) { }
        if (true) { }
        if (true) { }
        if (true) { }

        if (false) { }

        var b = 10 + 15;
        var d = 10 + 15;
        var e = 10 + 15;
        var c = 15;
        ;

        Console.WriteLine(a);
        Console.WriteLine(b);
        Console.WriteLine(c);
        Console.WriteLine(d);
        Console.WriteLine(e);
    }

    void Method()
    {
        var a = 5;
        Console.WriteLine(a);
    }
}
