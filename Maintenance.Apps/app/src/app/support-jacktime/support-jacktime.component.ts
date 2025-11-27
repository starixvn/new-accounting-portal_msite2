import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-support-jacktime',
  templateUrl: './support-jacktime.component.html',
  styleUrls: ['./support-jacktime.component.scss'],
  standalone: true,
  imports: [FormsModule],
})
export class SupportJacktimeComponent implements OnInit {
  filter: any = {
    matchId: null,
  };

  data: any[] = [];
  constructor() { }

  ngOnInit(): void {
  }

  loadData() {
    // Load data logic here
  }

}
