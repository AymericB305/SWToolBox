import { HttpInterceptorFn } from '@angular/common/http';
import {inject} from "@angular/core";
import {AuthService} from "./auth.service";
import {from, switchMap} from 'rxjs';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const supabase = inject(AuthService)

  return from(supabase.getSession()).pipe(
    switchMap(({ data }) => {
      const token = data.session?.access_token;
      if (token) {
        const cloned = req.clone({
          setHeaders: {
            Authorization: `Bearer ${token}`
          }
        });
        return next(cloned);
      }
      return next(req);
    })
  );
};
