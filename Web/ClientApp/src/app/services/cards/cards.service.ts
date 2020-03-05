import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from '../../../environments/environment'
import { Observable } from 'rxjs';
import { CardsEnum } from '../../typewriter/enums/CardsEnum.enum';

/**
 * Service for all cards API calls.
 */
@Injectable({
    providedIn: 'root'
})
export class CardsService {
    private apiController: string;

    constructor(private httpClient: HttpClient) {
        this.apiController = environment.apiUrl + '/Card';
    }

    /**
     * Get a shuffled deck of cards for the given game.
     * @param gametype Game to play.
     */
    getShuffledCards(gametype: number): Observable<CardsEnum[]> {
        const httpParams = new HttpParams()
            .set('gameType', gametype.toString());
        return this.httpClient.get<CardsEnum[]>(this.apiController + '/GetShuffledCards', { params: httpParams });
    }
}
