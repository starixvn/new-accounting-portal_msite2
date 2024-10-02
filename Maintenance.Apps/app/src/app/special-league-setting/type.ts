export type Option = {
    value: number;
    name: string;
}

export type League = {
    leagueId: number;
    leagueName: string;
    sportType: number;
    sportName: string;
    autoType: number | null;
    originalAutoType: number | null;
    autoOptions: Option[];
}

export type GetLeagueSettingRequest = {
    leagueId: number | null;
    autoType: number | null;
}



export type UpdateLeagueSettingRequest = {
    leagueId: number;
    autoType: number;
    sportType: number;
    action: string;
}