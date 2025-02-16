import { Routes } from '@angular/router';
import {AuthComponent} from "./pages/auth/auth.component";
import {IndexComponent} from "./pages/index/index.component";
import {authGuard} from "./shared/auth/auth.guard";

export const routes: Routes = [
  { path: 'auth', component: AuthComponent },
  { path: '', component: IndexComponent, canActivate: [authGuard] },
];
