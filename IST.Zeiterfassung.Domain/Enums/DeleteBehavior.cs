using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IST.Zeiterfassung.Domain.Enums
{
    public enum UserDeleteResult
    {
        NotFound = 0,
        PhysicallyDeleted = 1,
        LogicallyDeactivated = 2
    }


}
