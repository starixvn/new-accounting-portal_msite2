export interface MatchSettlementLock {
  matchId: number;
  sessionId: number;
  createdTime: string;
  homeTeam: string;
  awayTeam: string;
  totalSettlementProcessing: number;
  dbServer: string;
  serverName: string;
  errorMessage: string;
  messageId: string;
  isReplicated: boolean;
}

export interface DeleteSettlementLog {
  sessionId: number;
  dbServer: string;
  isSettleLock: boolean;
}

export interface UpdateSettlementLog {
  sessionId: number;
  createdTime: string;
  dbServer: string;
  messageId: string;
}
