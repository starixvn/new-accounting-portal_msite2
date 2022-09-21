export interface ParlayLockingRequest {
  matchId?: number;
  timeRange: number;
}

export interface MatchParlayLocking {
  serverName: string;
  dbServer: string;
  matchId: number;
  totalAffectedRefno: number;
  session: string;
  spName: string;
  createdDate: string;
}

export interface DeleteParlayLockingRequest {
  dbServer: string;
  session: string;
}
