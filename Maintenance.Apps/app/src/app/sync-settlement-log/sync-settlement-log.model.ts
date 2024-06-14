export interface SettlementErrorLog {
  serverName: string;
  dbServer: string;
  siteDBGroupId: number;
  logId: number;
  matchId: number;
  transInfo: string;
  eventName: string;
  sessionId: string;
  messageId: string;
  errorMessage: string;
  createdTime: Date;
  tableName: string;
  tempTableName: string;
  tableNames: string[];
  functionName: string;
  settlementAction: string;
  ticketType: string;
  errorMessages?: string[];
  ticketTypeExplicit: string;
}

export interface DetailSettlementErrorLog {
  id: number;
  ticketCount: number;
  functionName: string;
  tableName: string;
  ticketType: string;
  settlementAction: string;
  statusId: number;
  transIds: string;
  ticketTypeExplicit: string;
}
