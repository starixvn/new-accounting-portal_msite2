export interface MatchSettlementLock {
  matchId: number;
  sessionId: string;
  createdTime: string;
  homeTeam: string;
  awayTeam: string;
  totalSettlementProcessing: number;
  dbServer: string;
  errorMessage: string;
}

export interface DeleteSettlementLog {
  sessionId: string;
  dbServer: string;
  isSettleLock: boolean;
}

export interface UpdateSettlementLog {
  sessionId: string;
  createdTime: string;
  dbServer: string;
}
