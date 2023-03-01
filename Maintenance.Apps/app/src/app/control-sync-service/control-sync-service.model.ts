export interface LastLog {
    serverName: string;
    lastLogId: number;
    lastStatusId: number;
    statusName: string;
    currentStatus: boolean;
    waitingCount: number;
    queueType: number
}
