export interface DbInfo {
  tables: string[];
  dbSites: string[];
  subDbSites: string[];
}

export interface UpdateTransIdsRequest {
  TransIds: string;
  TableName: string;
  ServerName: string;
  ToServerNames: string[];
}

export interface Result {
  errCode: number;
  errMsg: string;
}
