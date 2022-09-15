export interface MatchSettlementLock {
  matchId: number;
  sessionId: string;
  createdTime: string;
  homeTeam: string;
  awayTeam: string;
  totalSettlementProcessing: number;
  dBServer: string;
  errorMessage: string;
}

export interface DeleteSettlementLog {
  sessionId: number;
  dbServer: string;
  isSettleLock: boolean;
}

export interface UpdateSettlementLog {
  sessionId: number;
  startTime: string;
  dbServer: string;
}
