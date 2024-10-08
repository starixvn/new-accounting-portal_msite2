import { inject, Injectable } from '@angular/core';
import { HttpService } from '../shared/services/http.service';
import {
    League,
    GetLeagueSettingRequest,
    UpdateLeagueSettingRequest,
} from './type';
import { BehaviorSubject, Observable } from 'rxjs';

interface ILeagueSettingService {
    setLeagues(leagues: League[] | null): void;
    getSetting(query: GetLeagueSettingRequest): Observable<League[]>;
    updateSetting(data: any): any;
}

@Injectable()
export class LeagueSettingService implements ILeagueSettingService {
    private readonly baseUrl = 'api/delete-temp-variable';
    private readonly httpService = inject(HttpService);

    private readonly leagues$ = new BehaviorSubject<League[] | null>(null);
    public leagues = this.leagues$.asObservable();

    setLeagues(leagues: League[] | null) {
        this.leagues$.next(leagues);
    }

    getLeagueById(leagueId: number) {
        return this.leagues$.value?.find((league) => league.leagueId === leagueId);
    }

    setLeagueById(leagueId: number, autoType: number | null) {
        const leagues = this.leagues$.value;
        if (!leagues) {
            return;
        }

        const newLeagues = leagues.map((league) =>
            league.leagueId === leagueId
                ? { ...league, autoType, originalAutoType: autoType }
                : league
        );

        this.setLeagues(newLeagues);
    }

    getSetting(query: GetLeagueSettingRequest) {
        const url = `${this.baseUrl}/league-setting`;
        return this.httpService.post<League[]>(url, query);
    }

    updateSetting(data: UpdateLeagueSettingRequest) {
        const url = `${this.baseUrl}/update-league-setting`;
        return this.httpService.post(url, data);
    }

    removeSetting(data: UpdateLeagueSettingRequest) {
        const url = `${this.baseUrl}/remove-league-setting`;
        return this.httpService.post(url, data);
    }
}
