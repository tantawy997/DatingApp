import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { take } from 'rxjs';
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
  user: User = {} as User;
  private loggedIn: boolean = false;
  users: Member[] = [];
  // username: string;
  constructor(
    public router: Router,
    public AccountService: AccountService,
    private MemberService: MemberService
  ) {
    this.AccountService.CurrentUser$.pipe(take(1)).subscribe((response) => {
      if (response) this.user = response;
    });
  }
  ngOnInit() {
    this.AccountService.CurrentUser$.subscribe((response) => {
      if (response) {
        if (response) this.user = response;
      }
    });
  }

  login() {
    this.AccountService.login(this.user).subscribe((response) => {
      window.location.reload();
      //this.router.navigateByUrl('/members/MembersList');
      this.user = {} as User;
    });
  }

  logout() {
    this.AccountService.logout();
    this.router.navigateByUrl('/');
  }

  // getCurrentUser() {
  //   this.AccountService.CurrentUser$.subscribe((res) => {
  //     this.loggedIn = !!res;
  //   });
  // }
}
