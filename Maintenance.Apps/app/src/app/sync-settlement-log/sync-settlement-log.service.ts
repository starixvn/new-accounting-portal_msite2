import { Injectable } from "@angular/core";
import { ConfirmDialogService } from "../shared/components/confirm-dialog/confirm-dialog.service";
import { HttpService } from "../shared/services/http.service";
import { SettlementErrorLog } from "./sync-settlement-log.model";

@Injectable()
export class SettlementErrorLogService {
    private baseUrl = 'api/delete-temp-variable';

    constructor(
        private httpService: HttpService,
        public dialogService: ConfirmDialogService
    ) { }


    public getAll(matchId: number) {

        const url = `${this.baseUrl}/get-settlement-error-logs`;
        const body = {
            matchId,
        };
        return this.httpService.post<SettlementErrorLog[]>(url, body);
    }

    public getDetails(sessionId: string) {
        const url = `${this.baseUrl}/get-details-settlement-error-log`;
        const body = {
            sessionId,
        };
        return this.httpService.post<SettlementErrorLog>(url, body);
    }

    public deleteInLogSettlementLock() { }

    public deleteInSettlementProcessing() { }

    public updateInSettlementProcessing() { }

    public deleteInSSBSettlementLogs() { }
}