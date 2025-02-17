import {ChangeDetectionStrategy, Component} from '@angular/core';
import { RouterOutlet } from '@angular/router';
import {MenuBarComponent} from "./shared/components/menu-bar/menu-bar.component";

@Component({
  selector: 'app-root',
  imports: [
    RouterOutlet,
    MenuBarComponent,
  ],
  templateUrl: './app.component.html',
  styles: ``,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AppComponent {
  title = 'SWToolBox-app';
}
