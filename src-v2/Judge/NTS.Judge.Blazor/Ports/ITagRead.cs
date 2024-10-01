using NTS.Domain.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Judge.Blazor.Ports;
public interface ITagRead
{
    Task<IEnumerable<RfidSnapshot>> ReadTags();
}
