import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { lastValueFrom } from 'rxjs';
import { DeleteTempVariableNumberDetailComponent } from './delete-temp-number-detail.component';
import { Match } from './match.model';
import { DeleteTempVariableNumberService } from './delete-temp-variable-number.service';

@Component({
  selector: 'app-delete-temp-variable-number',
  templateUrl: './delete-temp-variable-number.component.html',
  styleUrls: ['./delete-temp-variable-number.component.scss'],
})
export class DeleteTempVariableNumberComponent implements OnInit {
  constructor(
    private dialog: MatDialog,
    private service: DeleteTempVariableNumberService
  ) {}

  public matches: Match[] | null = null;
  public matchId!: string;

  ngOnInit(): void {
    this.service.dataUpdate$.subscribe((res) => {
      this.loadData();
      this.dialog.closeAll();
    });
  }

  loadData() {
    this.service.getAll(this.matchId).subscribe((data: Match[]) => {
      this.matches = data;
    });
  }

  submit() {
    this.loadData();
  }

  async openDetailMatchDialog(data: Match) {
    const result = await lastValueFrom(this.service.getDetail(data));
    const config = {
      data: result,
      width: '600px',
    };

    this.dialog.open(DeleteTempVariableNumberDetailComponent, config);
  }
}
