using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fanex.Data;

namespace Maintenance.DeleteTempVariable.Requests;

public class AmendmentResultUpdateRequest
{
    public List<AmendmentResultItem> Results { get; set; }
}

public class AmendmentResultItem
{
    public int MatchId { get; set; }
    public string SessionId { get; set; }
    public string DbServer { get; set; }
}