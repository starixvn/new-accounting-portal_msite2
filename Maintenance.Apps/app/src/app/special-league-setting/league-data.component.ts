import { Component, inject, OnInit } from '@angular/core';
import { League } from './type';
import { LeagueSettingService } from './league-setting.service';
import { AutoOptions } from './constants';
import { ConfirmDialogService } from '../shared/components/dialog/dialog.service';
import { SnackbarService } from '../shared/services/snackbar.service';

@Component({
    selector: 'league-data',
    templateUrl: './league-data.component.html',
})
export class LeagueDataComponent implements OnInit {
    private readonly leagueSettingService = inject(LeagueSettingService);
    private readonly dialogService = inject(ConfirmDialogService);
    private readonly snackbarService = inject(SnackbarService);

    public autoOptions = AutoOptions;
    public leagues: League[] | null = null;

    ngOnInit(): void {
        this.leagueSettingService.leagues.subscribe((leagues) => {
            this.leagues = leagues;
        });
    }

    update(league: League) {
        this.leagueSettingService
            .updateSetting({ leagueId: league.leagueId, autoType: league.autoType })
            .subscribe((res) => {
                if (res) {
                    this.snackbarService.openSuccessSnackBar();
                    this.leagueSettingService.setLeagueById(
                        league.leagueId,
                        league.autoType
                    );
                }
            });
    }

    openUpdateConfirmDialog(league: League) {
        this.dialogService
            .openConfirm({
                message: `
                <div>[${league.leagueId}]${league.leagueName}</div>
                <div>Are you sure you want to update Auto Type from ${league.originalAutoType} update to ${league.autoType} this row?</div>`,
            })
            .afterClosed()
            .subscribe((res) => {
                res && this.update(league);
            });
    }
}
