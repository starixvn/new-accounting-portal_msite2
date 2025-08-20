export interface Sport {
    sportType: number;
    sportName: string;
}

export interface ProviderInfo {
    sourceId: number;
    sourceName: string;
    remark: string;
    isActive: boolean;
    sportType: number;
    gameType: number;
    gameName: string;
}

export interface ProviderSport {
    sport: Sport;
    providers: ProviderInfo[];
}

export interface MapProvider {
    sportId: number;
    providerId: number;
}

export class Game {
    public gameType: number;
    public name: string;
    public isActive: boolean = true;

    constructor(gameType: number, name: string) {
        this.gameType = gameType;
        this.name = name;
    }
}

export const DefaultGames = [
    new Game(1, 'DOTA2'),
    new Game(2, 'LOL'),
    new Game(3, 'CS:GO'),
    new Game(4, 'KOG'),
    new Game(5, 'Wild Rift'),
    new Game(99, 'Others'),
];