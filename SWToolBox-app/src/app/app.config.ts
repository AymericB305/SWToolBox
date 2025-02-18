import {
  ApplicationConfig,
  inject,
  provideAppInitializer,
  provideExperimentalZonelessChangeDetection
} from '@angular/core';
import {provideRouter, withComponentInputBinding} from '@angular/router';

import { routes } from './app.routes';
import {provideHttpClient, withInterceptors} from "@angular/common/http";
import {authInterceptor} from "./shared/auth/auth.interceptor";
import {provideAnimationsAsync} from "@angular/platform-browser/animations/async";
import {providePrimeNG} from "primeng/config";
import Aura from '@primeng/themes/aura';
import {AuthService} from "./shared/auth/auth.service";
import {MeService} from "./shared/services/me.service";
import {EMPTY, firstValueFrom} from "rxjs";

export const appConfig: ApplicationConfig = {
  providers: [
    provideExperimentalZonelessChangeDetection(),
    provideRouter(routes, withComponentInputBinding()),
    provideHttpClient(withInterceptors([authInterceptor])),
    provideAnimationsAsync(),
    providePrimeNG({
      theme: {
        preset: Aura
      }
    }),
    provideAppInitializer(async () => {
      const authService = inject(AuthService);
      const meService = inject(MeService);

      if (await authService.isLoggedIn()) {
        return firstValueFrom(meService.loadMe());
      }
      return EMPTY
    })
  ]
};
