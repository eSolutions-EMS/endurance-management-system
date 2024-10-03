using NTS.Domain.Setup.Entities;
using NTS.Persistence.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Persistence.Adapters;
public class HorseRepository : SetRepository<Horse, SetupState>
{
    public HorseRepository(IStore<SetupState> store) : base(store)
    {
    }
}
