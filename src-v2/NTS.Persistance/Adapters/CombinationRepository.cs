using NTS.Domain.Setup.Entities;
using NTS.Persistence.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Persistence.Adapters;
public class CombinationRepository : SetRepository<Combination, SetupState>
{
    public CombinationRepository(IStore<SetupState> store) : base(store)
    {
    }
}
