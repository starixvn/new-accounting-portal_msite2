export interface DbInfo {
  tables: string[];
  dbSites: string[];
  subDbSites: string[];
}

export interface UpdateTransIdsRequest {
  transIds: string;
  tableName: string;
  serverName: string;
  toServerNames: string[];
}

export interface Result {
  errCode: number;
  errMsg: string;
}

export type SynMatch = {
  rowNumber: number;
  minTransId: number;
  maxTransId: number;
  matchId: number;
  tableName: string;
  serverName: string;
};

export type GetBatchTransIdRequest = {
  matchId: number;
  tableName: string;
  typeUpdate: string;
  serverName: string;
};

export type UpdateBatchTransIdRequest = {
  tableName: string;
  serverName: string;
  matchId: number;
  fromTransId: number;
  toTransId: number;
  toServerNames: string[];
};
