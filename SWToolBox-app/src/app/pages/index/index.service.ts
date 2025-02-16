import {inject, Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {environment} from "../../../environments/environment";

export interface GetMeResponse {
  id: string;
  name: string;
  guilds: GuildResponse[];
}

export interface GuildResponse {
  id: string;
  name: string;
  joinedAt: Date;
  leftAt?: Date | null;
  rank: RankResponse;
}

export interface RankResponse {
  id: number;
  name: string;
}

@Injectable({
  providedIn: 'root'
})
export class IndexService {

  private http = inject(HttpClient)
  private apiUrl = environment.apiUrl;
  private meUri = environment.meUri;

  getMe(): Observable<GetMeResponse> {
    return this.http.get<GetMeResponse>(`${this.apiUrl}/${this.meUri}`);
  }
}
