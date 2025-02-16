import {ChangeDetectionStrategy, Component, inject, signal} from '@angular/core';
import {AuthService} from "../../shared/auth/auth.service";
import {FormsModule} from "@angular/forms";
import { ButtonModule } from 'primeng/button';
import {CardModule} from "primeng/card";
import {InputTextModule} from "primeng/inputtext";
import {PasswordModule} from "primeng/password";
import {MessageModule} from "primeng/message";
import {Router} from "@angular/router";

@Component({
  selector: 'app-auth',
  imports: [
    ButtonModule,
    CardModule,
    InputTextModule,
    PasswordModule,
    FormsModule,
    MessageModule,
  ],
  templateUrl: './auth.component.html',
  styles: ``,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AuthComponent {

  private service = inject(AuthService);
  private router = inject(Router);

  isSignUp = signal(false);
  email = signal('');
  password = signal('');
  confirmPassword = signal('');
  errorMessage = signal('');

  toggleSignUp() {
    this.isSignUp.set(!this.isSignUp());
    this.errorMessage.set('');
  }

  async onSubmit() {
    this.errorMessage.set('');

    if (this.isSignUp()) {
      await this.signUp();
      return;
    }
    await this.signIn();
  }

  private async signIn() {
    try {
      await this.service.signInWithPassword(this.email(), this.password());
      await this.router.navigate(['/']);
    } catch (error) {
      this.errorMessage.set('Failed to sign in. Please check your credentials.');
    }
  }

  private async signUp() {
    if (this.password() !== this.confirmPassword()) {
      this.errorMessage.set('Passwords do not match.');
      return;
    }
    try {
      await this.service.signUp(this.email(), this.password());
    } catch (error) {
      this.errorMessage.set('Failed to sign up. Please try again.');
    }
  }
}
