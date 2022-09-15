import { Component, OnInit } from '@angular/core';
import { HttpService } from '../shared/services/http.service';

interface Filter {
  matchId?: number;
  timeRange: number;
}

interface MatchParlayLocking {
  matchId: number;
  totalAffectedRefno: number;
  session: string;
  sPName: string;
  createdDate: string;
}

@Component({
  selector: 'app-delete-parlay-locking',
  templateUrl: './delete-parlay-locking.component.html',
  styleUrls: ['./delete-parlay-locking.component.scss'],
})
export class DeleteParlayLockingComponent implements OnInit {
  public filter: Filter = { timeRange: 1 };
  public timeRanges: number[] = [1, 2, 3];
  public matches: MatchParlayLocking[] | null = null;

  constructor(private httpService: HttpService) {}

  ngOnInit(): void {}

  submit() {
    this.httpService
      .post<MatchParlayLocking[]>(
        'api/delete-temp-variable/get-parlay-locking',
        this.filter
      )
      .subscribe((data) => {
        this.matches = data.length
          ? data
          : [
              {
                matchId: 1,
                totalAffectedRefno: 1,
                session: 'ffefe',
                sPName: 'fefe',
                createdDate: 'fafa',
              },
            ];
      });
  }

  onDelete(session: string) {
    // this.httpService
    //   .post('api/delete-temp-variable/delete-parlay-locking', {
    //     session,
    //   })
    //   .subscribe((data) => {
    //     console.log(data);
    //   });
  }
}
