import {ChangeDetectionStrategy, Component, computed, inject} from '@angular/core';
import {TableModule} from "primeng/table";
import {GuildService} from "../../guild.service";
import {DatePipe} from "@angular/common";
import {TreeTableModule} from "primeng/treetable";

@Component({
  selector: 'app-members',
  imports: [
    TableModule,
    DatePipe,
    TreeTableModule
  ],
  templateUrl: './members.component.html',
  styles: ``,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class MembersComponent {

  private guildService = inject(GuildService);

  members = computed(() => this.guildService.members())
  towers = computed(() => this.guildService.towers())
  // treeData = computed(() => this.towers().map(tower => ({
  //   data: {
  //     name: tower.name,
  //   },
  //   children: tower.defenses.map(defense => ({
  //     data: {
  //       monsterLead: defense.monsterLead.name,
  //       monster2: defense.monster2.name,
  //       monster3: defense.monster3.name,
  //       description: defense.description,
  //       player: defense.player,
  //       wins: defense.wins,
  //       losses: defense.losses,
  //     },
  //   })),
  // })))
}
