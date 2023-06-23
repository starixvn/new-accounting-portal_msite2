export interface DbInfo {
    tables: string[];
    dbSites: string[];
}

export interface UpdateTransIdsRequest {
    TransIds: string;
    TableName: string;
    ServerName: string;
}

export interface Result {
    errCode: number;
    errMsg: string;
}