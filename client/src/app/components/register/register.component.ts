import { Component, Output, EventEmitter } from '@angular/core';
import { Router } from '@angular/router';
import { User } from 'src/app/Models/user';
import { AccountService } from 'src/app/Services/Account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css'],
})
export class RegisterComponent {
  user: User = { id: 0, userName: '', password: '' };
  @Output() cancelRegister = new EventEmitter();

  /**
   *
   */
  constructor(public AccountService: AccountService, private router: Router) {}
  cancel() {
    this.cancelRegister.emit(false);
  }

  signUp() {
    this.AccountService.register(this.user).subscribe(
      (response) => {
        console.log(response);
        this.cancel();
      },
      (e) => {
        console.log(e.message);
      }
    );

    this.router.navigateByUrl('home');
  }
}
