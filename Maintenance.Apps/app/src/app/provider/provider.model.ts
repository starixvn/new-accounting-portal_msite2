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
    id: number;
    sportId: number;
    providerId: number;
    providerName: string;
    sportName: string;
    createdDate: string;
    createdBy: string;
    updatedDate: string;
    updatedBy: string;
}