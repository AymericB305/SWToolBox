import {inject, Injectable} from '@angular/core';
import {environment} from "../../../environments/environment";
import {HttpClient} from "@angular/common/http";
import {toSignal} from "@angular/core/rxjs-interop";

export interface MeState {
  id: string;
  name: string;
  guilds: Guild[];
}

interface Guild {
  id: string;
  name: string;
  joinedAt: Date;
  leftAt?: Date | null;
  rank: Rank;
}

interface Rank {
  id: number;
  name: string;
}

@Injectable({
  providedIn: 'root'
})
export class MeService {

  private http = inject(HttpClient);
  private apiUrl = environment.apiUrl;
  private meUri = environment.meUri;

  me = toSignal(this.http.get<MeState>(`${this.apiUrl}/${this.meUri}`));

  constructor() { }
}
