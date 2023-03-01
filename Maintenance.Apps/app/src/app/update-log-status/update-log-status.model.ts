export interface LogStatus {
    serverName: string;
    dbServer: string;
    logId: number;
    statusId: number;
    statusName: string;
    startTime: string;
    endTime: string;
    startTimeValidation: string;
    systemName: string;
    functionName: string;
    tableName: string;
    action: string;
    matchId: number;
    transInfo: string;
    transInfoJson: string;
    logDate: string;
    settlementAction: string;
    ticketType: string;

    queueType: number;
}

export interface UpdateLogStatusRequest {
    dbServer: string;
    logId: number;
    action: 'Update' | 'Delete';
}
