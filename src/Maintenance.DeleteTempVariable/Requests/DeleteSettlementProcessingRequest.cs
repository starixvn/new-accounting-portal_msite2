using System;

namespace Maintenance.DeleteTempVariable.Requests
{
    public class DeleteSettlementProcessingRequest
    {
        public Int64 SessionId { get; set; }

        public string DbServer { get; set; }

        public bool isSettleLock { get; set; }
    }
}