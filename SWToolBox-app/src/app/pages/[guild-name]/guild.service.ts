import {computed, inject, Injectable, signal} from '@angular/core';
import {Observable, tap} from "rxjs";
import {HttpClient} from "@angular/common/http";
import {environment} from "../../../environments/environment";

export interface Guild {
  id: string;
  name: string;
  members: Member[];
  defenses: Defense[];
}

interface Member {
  id: string;
  name: string;
  joinedAt: Date;
  leftAt: Date | null;
  rank: Rank;
}

interface Rank {
  id: number;
  name: string;
}

interface Defense {
  id: string;
  monsterLead: Monster;
  monster2: Monster;
  monster3: Monster;
  description: string;
  placements: Placement[];
}

interface Monster {
  id: number;
  name: string;
}

interface Placement {
  playerId: string;
  tower: Location;
  wins: number;
  losses: number;
}

interface Location {
  id: number;
  name: string;
}

interface Tower {
  id: number;
  name: string;
  defenses: PlacedDefense[];
}

interface PlacedDefense {
  id: string;
  monsterLead: Monster;
  monster2: Monster;
  monster3: Monster;
  description: string;
  player: Member;
  wins: number;
  losses: number;
}

@Injectable({
  providedIn: 'root'
})
export class GuildService {
  private http = inject(HttpClient);
  private apiUrl = environment.apiUrl;
  private guildsUri = environment.guildsUri;

  // state
  private state = signal<Guild>({
    id: '',
    name: '',
    defenses: [],
    members: [],
  });

  // selectors
  members = computed(() => this.state().members);
  defenses = computed(() => this.state().defenses);
  towers = computed(() => {
    const groupedByTower = this.defenses().reduce((acc, defense) => {
      defense.placements.forEach(placement => {
        const tower = placement.tower;

        // create the tower if needed
        if (!acc[tower.id]) {
          acc[tower.id] = {
            id: tower.id,
            name: tower.name,
            defenses: [],
          };
        }

        // populate its defenses
        acc[tower.id].defenses.push({
          id: defense.id,
          monsterLead: defense.monsterLead,
          monster2: defense.monster2,
          monster3: defense.monster3,
          description: defense.description,
          player: this.members().find(m => m.id === placement.playerId)!,
          wins: placement.wins,
          losses: placement.losses,
        });
      });
      return acc;
    }, {} as Record<number, Tower>);

    return Object.values(groupedByTower);
  });

  // actions
  loadGuild(guildId: string): Observable<Guild> {
    return this.http.get<Guild>(`${this.apiUrl}/${this.guildsUri}/${guildId}`)
      .pipe(tap(guild => this.state.set(guild)))
  }
}
