import { ResolveFn } from '@angular/router';
import {inject} from "@angular/core";
import {MeService} from "../../shared/services/me.service";
import {Guild, GuildService} from "./guild.service";
import {EMPTY} from "rxjs";

export const guildResolver: ResolveFn<Guild> = (route) => {
  const meService = inject(MeService);
  const guildService = inject(GuildService);

  const guild = meService.me()?.guilds?.find(g => g.name === route.paramMap.get('guildName'));

  if (guild) {
    return guildService.loadGuild(guild.id)
  }

  return EMPTY;
};
