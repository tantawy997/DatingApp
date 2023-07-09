import { Component, OnInit } from '@angular/core';
import { AccountService } from './Services/Account.service';
import { User } from './Models/user';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent implements OnInit {
  title = 'client';

  constructor(private AccountService: AccountService) {}
  ngOnInit(): void {
    this.SetCurrentUser();
  }

  SetCurrentUser() {
    let userString = localStorage.getItem('user');
    if (!userString) return;
    const user: User = JSON.parse(userString);

    this.AccountService.setCurrentUser(user);
  }
}
