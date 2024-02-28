import { Component, OnInit } from '@angular/core';
import { ConfirmDialogService } from '../shared/components/dialog/dialog.service';
import { ResultAmendmentService } from './result-amendment.service';
import { ResultAmendment } from './type';

@Component({
  selector: 'app-result-amendment',
  templateUrl: './result-amendment.compoment.html',
})
export class ResultAmendmentCompoment implements OnInit {
  ngOnInit(): void {
    this.loadData();
  }

  public results: ResultAmendment[] = [];

  constructor(
    private dialogService: ConfirmDialogService,
    private resultAmendmentService: ResultAmendmentService
  ) {}

  loadData() {
    this.resultAmendmentService.getAmenentResults().subscribe((res) => {
      this.results = res;
    });
  }

  public openConfirmDialog() {
    const message = `Are you sure you want to change selected sessions' statuses to '2':"Completed input Tickets data"?`;
    const title = 'Confirm Change Session Status';

    this.dialogService
      .openConfirm({ title, message })
      .afterClosed()
      .subscribe((confirm: Boolean) => {
        if (confirm) {
          this.handleChangeStatus();
        }
      });
  }

  private handleChangeStatus() {
    const selectedResults = this.results.filter((x) => x.selected);
    this.resultAmendmentService
      .updateStatus({
        results: selectedResults,
      })
      .subscribe(() => {
        this.loadData();
      });
  }
}
