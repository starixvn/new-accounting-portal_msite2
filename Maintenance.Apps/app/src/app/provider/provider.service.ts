import { inject, Injectable, Provider } from '@angular/core';
import { HttpService } from '../shared/services/http.service';
import { ProviderInfo, Sport } from './provider.model';

@Injectable({
    providedIn: 'root'
})
export class ProviderService {
    private baseUrl = 'api/delete-temp-variable';
    private readonly http = inject(HttpService);
    constructor() { }


    getAllSport() {
        return this.http.getData<Sport[]>(`${this.baseUrl}/sport`);
    }

    getProvidersBySport(sportId: number) {
        return this.http.getData<ProviderInfo[]>(`${this.baseUrl}/get-providers/${sportId}`);
    }

    mapProviderToSport(providerId: number, sportId: number) {
        return this.http.post('/api/provider/map-provider-to-sport', { providerId, sportId });
    }

    unmapProviderFromSport(providerId: number, sportId: number) {
        return this.http.post('/api/provider/unmap-provider-from-sport', { providerId, sportId });
    }
}
