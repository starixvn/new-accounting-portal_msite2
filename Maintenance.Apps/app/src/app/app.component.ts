import { Component, OnInit } from '@angular/core';
import { BlockUIService } from './shared/services/block-ui.service';
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent implements OnInit {
  title = 'app';
  public blockUI = true;
  constructor(private blockUIService: BlockUIService) {}
  public ngOnInit() {
    this.blockUIService.blockUIEvent.subscribe(
      (event: boolean) => (this.blockUI = event)
    );
  }
}
