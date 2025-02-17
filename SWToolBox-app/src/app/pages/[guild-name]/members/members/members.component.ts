import {ChangeDetectionStrategy, Component, effect, inject, input, linkedSignal} from '@angular/core';
import {MeService} from "../../../../shared/services/me.service";
import {HttpClient} from "@angular/common/http";
import {rxResource} from "@angular/core/rxjs-interop";
import {EMPTY} from "rxjs";

@Component({
  selector: 'app-members',
  imports: [],
  templateUrl: './members.component.html',
  styles: ``,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class MembersComponent {
  guildName = input.required<string>();

  private meService = inject(MeService);

  me = this.meService.me;

  guild = linkedSignal({
    source: () => ({ guildName: this.guildName(), me: this.me() }),
    computation: ({ guildName, me }) => {
      return me?.guilds?.find(g => g.name === guildName);
    }
  });

  private http = inject(HttpClient);

  guildData = rxResource({
    request: this.guild,
    loader: ({ request }) => {
      if (request) {
        return this.http.get('http://localhost:5178/api/v1/guilds/' + request.id);
      }
      return EMPTY;
    },
  });

  a = effect(() => {
    console.log("guild", this.guild())
    console.log("data", this.guildData.value())
  });
}
