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
    let user: User = JSON.parse(localStorage.getItem('user')!);

    this.AccountService.setCurrentUser(user);
  }
}
