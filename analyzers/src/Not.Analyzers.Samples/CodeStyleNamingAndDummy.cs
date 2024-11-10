using System; // Sohuld not have unused usings
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Judge.Blazor.CodeStyle;

public class CodeStyleNamingAndDummy // Cannot have And in class name
{
    bool _fieldAndName;
    private bool PrivatePropertyAndName { get; }

    public bool PublicPorpertyAndName { get; }

    void PrivateMethodAndName() { }

    public void PublicMethodAndName() { }
}
