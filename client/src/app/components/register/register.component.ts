import { Component, Output, EventEmitter } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Member } from 'src/app/Models/member';
import { User } from 'src/app/Models/user';
import { AccountService } from 'src/app/Services/Account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css'],
})
export class RegisterComponent {
  user: User = {} as User;
  @Output() cancelRegister = new EventEmitter();

  /**
   *
   */
  constructor(
    public AccountService: AccountService,
    private router: Router,
    private toaster: ToastrService
  ) {}
  cancel() {
    this.cancelRegister.emit(false);
  }

  signUp() {
    this.AccountService.register(this.user).subscribe(
      (response) => {
        console.log(response);
        this.toaster.success('you registered successfully');
        this.cancel();
      },
      (e) => {
        this.toaster.error(e.status);
      }
    );

    this.router.navigateByUrl('home');
  }
}
