import { Component, OnInit } from '@angular/core';
import { Observable, take } from 'rxjs';
import { Member } from 'src/app/Models/member';
import { Pagination } from 'src/app/Models/pagination';
import { User } from 'src/app/Models/user';
import { UserParams } from 'src/app/Models/user-params';
import { AccountService } from 'src/app/Services/Account.service';
import { MemberService } from 'src/app/Services/member.service';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css'],
})
export class MemberListComponent implements OnInit {
  members: Member[] = [];
  //members$: Observable<Member[]> | undefined;
  pagination: Pagination | undefined;
  user: User | undefined;
  userParam: UserParams | undefined;
  constructor(
    private MemberService: MemberService,
    private accountService: AccountService
  ) {
    this.userParam = this.MemberService.getUserParams();
  }
  ngOnInit() {
    this.LoadMembers();
  }

  LoadMembers() {
    if (this.userParam) {
      this.MemberService.setUserParams(this.userParam);
      this.MemberService.GetMembers(this.userParam).subscribe(
        (response: any) => {
          if (response.Pagination && response.result) {
            this.pagination = response.Pagination;
            this.members = response.result;
          }
        }
      );
    }
  }
  identify(index: number, item: Member) {
    return item.userId;
  }

  pageChanged(event: any) {
    if (this.userParam && this.userParam?.pageNumber !== event.page) {
      this.userParam.pageNumber = event.page;
      this.MemberService.setUserParams(this.userParam);
      this.LoadMembers();
    }
  }

  ResetFilters() {
    this.userParam = this.MemberService.ResetUSerParams();
    this.LoadMembers();
  }
}
