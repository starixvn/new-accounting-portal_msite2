using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maintenance.DeleteTempVariable.Constants
{
    public enum AmendmentStatus : short
    {
        Running = 1,
        Complete = 2,
        TimeOut = 3,
        FailSettle = 99
    }
}