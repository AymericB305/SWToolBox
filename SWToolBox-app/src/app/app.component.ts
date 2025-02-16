import {ChangeDetectionStrategy, Component, inject, linkedSignal} from '@angular/core';
import { RouterOutlet } from '@angular/router';
import {GetMeResponse, IndexService} from "./pages/index/index.service";
import {toSignal} from "@angular/core/rxjs-interop";
import {SupabaseService} from "./shared/services/supabase.service";

@Component({
    selector: 'app-root',
    imports: [RouterOutlet],
    templateUrl: './app.component.html',
    styleUrl: './app.component.css',
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AppComponent {
  title = 'SWToolBox-app';

  private service = inject(IndexService)
  private supabase = inject(SupabaseService)
  me: GetMeResponse | undefined = undefined

  constructor() {
    this.supabase.signInWithPassword("", "").then(r => {})
  }

  click() {
    this.service.getMe().subscribe((me) => this.me = me);
  }
}
