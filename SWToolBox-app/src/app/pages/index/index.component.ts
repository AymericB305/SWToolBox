import {ChangeDetectionStrategy, Component, inject} from '@angular/core';
import {MeService} from "../../shared/services/me.service";

@Component({
  selector: 'app-index',
  imports: [],
  templateUrl: './index.component.html',
  styles: ``,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class IndexComponent {

  private meService = inject(MeService);
  me = this.meService.me;

}
