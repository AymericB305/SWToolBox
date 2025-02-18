import {computed, inject, Injectable, signal} from '@angular/core';
import {environment} from "../../../environments/environment";
import {HttpClient} from "@angular/common/http";
import {tap} from "rxjs";

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

  private writableMe = signal<MeState | undefined>(undefined);
  me = computed(() => this.writableMe());

  loadMe() {
    return this.http.get<MeState>(`${this.apiUrl}/${this.meUri}`)
      .pipe(tap(me => this.writableMe.set(me)));
  }
}
