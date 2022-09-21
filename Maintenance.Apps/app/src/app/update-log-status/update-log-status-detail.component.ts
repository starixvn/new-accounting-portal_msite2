import { Component, Inject, OnInit, ViewEncapsulation } from '@angular/core';
import { MatDialog, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ActivatedRoute } from '@angular/router';
import { LogStatus } from './update-log-status.model';
import { UpdateLogStatusService } from './update-log-status.service';

@Component({
  selector: 'app-update-log-status-detail',
  templateUrl: './update-log-status-detail.component.html',
  styleUrls: ['./update-log-status-detail.component.scss'],
  encapsulation: ViewEncapsulation.None,
})
export class UpdateLogStatusDetailComponent implements OnInit {
  constructor(
    private updateLogStatusService: UpdateLogStatusService,
    @Inject(MAT_DIALOG_DATA) public data: LogStatus
  ) {}

  ngOnInit(): void {
    this.data.transInfo = `<root><r t="30484763043168256" s="running" sid="0" wld="2022-09-20T00:00:00" bdaq="40" r="0" w="0.0000" aw="0.0000" mw="0.0000" sw="0.0000" pc="0.0000" c="0.0000" ac="0.0000" sc="0.0000" statuswl="" bef_w="0.0000" bef_sid="0" /><r t="30486266281721856" s="running" sid="0" wld="2022-09-20T00:00:00" bdaq="40" r="0" w="0.0000" aw="0.0000" mw="0.0000" sw="0.0000" pc="0.0000" c="0.0000" ac="0.0000" sc="0.0000" statuswl="" bef_w="0.0000" bef_sid="0" /><r t="30486274871656448" s="running" sid="0" wld="2022-09-20T00:00:00" bdaq="40" r="0" w="0.0000" aw="0.0000" mw="0.0000" sw="0.0000" pc="0.0000" c="0.0000" ac="0.0000" sc="0.0000" statuswl="" bef_w="0.0000" bef_sid="0" /></root>`;
  }

  openUpdateConfirmDialog() {
    this.updateLogStatusService.openUpdateConfirmDialog(this.data);
  }

  openDeleteConfirmDialog() {
    this.updateLogStatusService.openDeleteConfirmDialog(this.data);
  }

  transformTransInfo(transInfo: string) {
    const parser = new DOMParser();
    const doc = parser.parseFromString(transInfo, 'application/xml');
    const rTag = doc.getElementsByTagName('r');
    return Array.from(rTag)
      .map((e) => e.getAttribute('t'))
      .join(',');
  }
}
