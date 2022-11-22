import { Injectable } from "@angular/core";
import { ConfirmDialogService } from "../shared/components/confirm-dialog/confirm-dialog.service";
import { HttpService } from "../shared/services/http.service";

@Injectable()
export class UpdateLogStatusService {
    private baseUrl = 'api/delete-temp-variable';

    constructor(
        private httpService: HttpService,
        public dialogService: ConfirmDialogService
    ) { }


}