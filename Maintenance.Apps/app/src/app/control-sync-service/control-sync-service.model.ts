export interface LastLog {
  serverName: string;
  lastLogId: number;
  lastStatusId: number;
  currentStatus: boolean;
  waitingCount: number;
}
