export interface Sport {
    sportType: number;
    sportName: string;
}

export interface ProviderInfo {
    sourceId: number;
    sourceName: string;
    remark: string;
    isActive: boolean;
}

export interface ProviderSport {
    sport: Sport;
    providers: ProviderInfo[];
}

export interface MapProvider {
    sportId: number;
    providerId: number;
}