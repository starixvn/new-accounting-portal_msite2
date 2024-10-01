import { Component, EventEmitter, inject, Output } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { LeagueSettingService } from './league-setting.service';
import { AutoOptions } from './constants';

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
            leagueId: new FormControl<string | null>(null),
            autoType: new FormControl<number | null>(null),
        });
    }

    onSubmit() {
        this.leagueSettingService
            .getSetting(this.searchForm.value)
            .subscribe((leagues) => {
                const newLeagues = leagues.map((league) => ({ ...league, autoOptions: AutoOptions }));
                this.leagueSettingService.setLeagues(newLeagues);
            });
    }
}
