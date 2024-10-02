import { Component, inject } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { LeagueSettingService } from './league-setting.service';
import {
  AutoOptions,
  BasketballAutoOptions,
  SoccerAutoOptions,
} from './constants';
import { SportType } from '../control-sync-service/constants';

@Component({
  selector: 'league-filter',
  templateUrl: './league-filter.component.html',
})
export class LeagueFilterComponent {
  private readonly leagueSettingService = inject(LeagueSettingService);
  public searchForm!: FormGroup;
  public autoOptions = AutoOptions;

  constructor() {
    this.searchForm = new FormGroup({
      leagueId: new FormControl<number | null>(null),
      autoType: new FormControl<number | null>(null),
    });
  }

  onSubmit() {
    this.leagueSettingService
      .getSetting(this.searchForm.value)
      .subscribe((leagues) => {
        const getOptions = (sportType: number) => {
          switch (sportType) {
            case SportType.soccer:
              return SoccerAutoOptions;
            case SportType.basketBall:
              return BasketballAutoOptions;
            default:
              return AutoOptions;
          }
        };
        const newLeagues = leagues.map((league) => ({
          ...league,
          autoOptions: getOptions(league.sportType),
        }));
        this.leagueSettingService.setLeagues(newLeagues);
      });
  }
}
