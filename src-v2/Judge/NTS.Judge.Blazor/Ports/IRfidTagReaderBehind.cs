using Not.Injection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Judge.Blazor.Ports;
public interface IRfidTagReaderBehind : ISingletonService
{
    Task ReadTags();
}
