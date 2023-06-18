import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { User } from 'src/app/Models/user';
import { AccountService } from 'src/app/Services/Account.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css'],
})
export class HeaderComponent {
  user: User = { id: 0, password: '', userName: '' };
  private loggedIn: boolean = false;
  constructor(public router: Router, public AccountService: AccountService) {}

  login() {
    console.log(this.user);
    this.AccountService.login(this.user);
  }

  logout() {
    this.AccountService.logout();
  }

  getCurrentUser() {
    this.AccountService.CurrentUser.subscribe((res) => {
      this.loggedIn = !!res;
    });
  }
}
