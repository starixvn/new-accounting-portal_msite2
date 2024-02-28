export type ResultAmendment = {
  matchId: number;
  statusId: number;
  sessionId: string;
  createdDate: Date;
  dbServer: string;
  selected: boolean;
};

export interface AmendmentResultUpdateRequest {
  results: ResultAmendment[];
}
