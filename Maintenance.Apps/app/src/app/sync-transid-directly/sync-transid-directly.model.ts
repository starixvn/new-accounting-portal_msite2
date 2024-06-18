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
  isSuccess: boolean;
}

export type SynMatch = {
  rowNumber: number;
  minTransId: string;
  maxTransId: string;
  matchId: number;
  tableName: string;
  serverName: string;
  isUpdated: boolean;
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
  fromTransId: string;
  toTransId: string;
  toServerNames: string[];
};
