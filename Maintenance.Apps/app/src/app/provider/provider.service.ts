import { inject, Injectable, Provider } from '@angular/core';
import { HttpService } from '../shared/services/http.service';
import { ProviderInfo, Sport } from './provider.model';

@Injectable({
    providedIn: 'root'
})
export class ProviderService {
    private readonly baseUrl = 'api/delete-temp-variable';
    private readonly http = inject(HttpService);


    getAllSport() {
        return this.http.getData<Sport[]>(`${this.baseUrl}/sport`);
    }

    getAllProvider() {
        return this.http.getData<ProviderInfo[]>(`${this.baseUrl}/get-all-providers`);
    }

    getProvidersBySport(sportId: number) {
        return this.http.getData<ProviderInfo[]>(`${this.baseUrl}/get-providers/${sportId}`);
    }

    mapProviderToSport(sourceId: number, sportType: number, gameType: string) {
        return this.http.post<{ isSuccess: boolean }>(`${this.baseUrl}/provider-mapping`, { sourceId, sportType, gameType, isActive: true });
    }

    unmapProviderFromSport(sourceId: number, sportType: number, gameType: string) {
        return this.http.post<{ isSuccess: boolean }>(`${this.baseUrl}/provider-mapping`, { sourceId, sportType, gameType, isActive: false });
    }
}
