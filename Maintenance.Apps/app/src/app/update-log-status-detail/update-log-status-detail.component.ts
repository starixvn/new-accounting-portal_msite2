import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Log } from '../update-log-status/log.model';

@Component({
  selector: 'app-update-log-status-detail',
  templateUrl: './update-log-status-detail.component.html',
  styleUrls: ['./update-log-status-detail.component.scss'],
})
export class UpdateLogStatusDetailComponent implements OnInit {
  public log!: Log;
  constructor(private activeRoute: ActivatedRoute) {}

  ngOnInit(): void {
    const id = this.activeRoute.snapshot.params['id'];
    console.log(id);
  }
}
