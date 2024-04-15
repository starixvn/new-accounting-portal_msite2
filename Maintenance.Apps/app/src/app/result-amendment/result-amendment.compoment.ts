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
  public filter = { matchId: null, statusId: 0 };

  constructor(
    private dialogService: ConfirmDialogService,
    private resultAmendmentService: ResultAmendmentService
  ) {}

  loadData() {
    this.resultAmendmentService
      .getAmenentResults({ ...this.filter })
      .subscribe((res) => {
        const filtered = res.filter(
          (x) =>
            +this.filter.statusId === 0 || x.statusId === +this.filter.statusId
        );
        this.results = res;
      });
  }

  public openConfirmDialog() {
    const message = `Are you sure you want to update selected sessions' statuses?`;
    const title = 'Confirm Change Session Status';
    const errorMessage =
      'Some of selected session has StatusID updated already. Please check then try again.';
    const selectedResults = this.results.filter((x) => x.selected);

    if (!selectedResults.length) {
      this.dialogService.openWarning({ message: errorMessage });
      return;
    }

    this.dialogService
      .openConfirm({ title, message })
      .afterClosed()
      .subscribe((confirm: Boolean) => {
        if (confirm) {
          this.handleChangeStatus(selectedResults);
        }
      });
  }

  private handleChangeStatus(selectedResults: ResultAmendment[]) {
    this.resultAmendmentService
      .updateStatus({
        results: selectedResults,
      })
      .subscribe(() => {
        this.loadData();
      });
  }

  submit() {
    this.loadData();
  }
}
