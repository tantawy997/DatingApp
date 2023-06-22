import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Member } from 'src/app/Models/member';
import { User } from 'src/app/Models/user';
import { AccountService } from 'src/app/Services/Account.service';
import { MemberService } from 'src/app/Services/member.service';
@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css'],
})
export class HeaderComponent implements OnInit {
  user: User = { id: 0, password: '', userName: '' };
  private loggedIn: boolean = false;
  users: Member[] = [];
  // username: string;
  constructor(
    public router: Router,
    public AccountService: AccountService,
    private MemberService: MemberService
  ) {}
  ngOnInit() {}

  login() {
    console.log(this.user);
    this.AccountService.login(this.user);
    this.router.navigateByUrl('/members/MembersList');
  }

  logout() {
    this.AccountService.logout();
    this.router.navigateByUrl('/');
  }

  getCurrentUser() {
    this.AccountService.CurrentUser.subscribe((res) => {
      this.loggedIn = !!res;
    });
  }
}
