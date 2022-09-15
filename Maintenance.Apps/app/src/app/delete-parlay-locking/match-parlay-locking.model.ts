export interface Filter {
  matchId?: number;
  timeRange: number;
}

export interface MatchParlayLocking {
  serverName: string;
  dbServer: string;
  matchId: number;
  totalAffectedRefno: number;
  session: string;
  sPName: string;
  createdDate: string;
}
