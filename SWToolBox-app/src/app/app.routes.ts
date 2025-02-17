import { Routes } from '@angular/router';
import {AuthComponent} from "./pages/auth/auth.component";
import {IndexComponent} from "./pages/index/index.component";
import {authGuard} from "./shared/auth/auth.guard";
import {MembersComponent} from "./pages/[guild-name]/members/members/members.component";

export const routes: Routes = [
  {
    path: 'auth',
    component: AuthComponent,
  },
  {
    path: '',
    component: IndexComponent,
    canActivate: [authGuard],
  },
  {
    path: ':guildName',
    canActivate: [authGuard],
    children: [
      {
        path: 'members',
        component: MembersComponent,
      },
    ],
  }
];
