export interface SettlementErrorLog {
    serverName: string;
    dbServer: string;
    logId: number;
    matchId: number;
    transInfo: string;
    eventName: string;
    sessionId: string;
    messageId: string;
    errorMessage: string;
    tableName: string;
}