using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fanex.Data;
using Fanex.Data.Repository;

namespace Maintenance.DeleteTempVariable.Criteria;

public class AmendmentResultSelCriteria : CriteriaBase
{
    public short StatusId { get; set; } = 0;

    public int? MatchId { get; set; }

    [SpParam(Ignored = true)]
    public string DbServer { get; set; }

    public override string GetSettingKey() => $"NAP_SettlementResultHistory_Sel_{DbServer}";

    public override bool IsValid() => !string.IsNullOrWhiteSpace(DbServer);
}