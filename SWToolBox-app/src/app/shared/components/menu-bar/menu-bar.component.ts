import {ChangeDetectionStrategy, Component, inject, linkedSignal} from '@angular/core';
import {MeService} from "../../services/me.service";
import {FormsModule} from "@angular/forms";
import {SelectModule} from "primeng/select";
import {ButtonModule} from "primeng/button";
import {FluidModule} from 'primeng/fluid';

@Component({
  selector: 'app-menu-bar',
  imports: [
    SelectModule,
    FormsModule,
    ButtonModule,
    FluidModule,
  ],
  templateUrl: './menu-bar.component.html',
  styles: ``,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class MenuBarComponent {

  private meService = inject(MeService);

  me = this.meService.me;

  selectedGuildId = linkedSignal({
    source: this.me,
    computation: (me) => {
      if (me?.guilds && me.guilds.length > 0) {
        return me.guilds[0].id;
      }
      return undefined;
    }
  });
}
