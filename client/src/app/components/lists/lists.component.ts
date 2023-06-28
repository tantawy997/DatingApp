import { Component, OnInit } from '@angular/core';
import { Member } from 'src/app/Models/member';
import { Pagination } from 'src/app/Models/pagination';
import { UserParams } from 'src/app/Models/user-params';
import { MemberService } from 'src/app/Services/member.service';

@Component({
  selector: 'app-lists',
  templateUrl: './lists.component.html',
  styleUrls: ['./lists.component.css'],
})
export class ListsComponent implements OnInit {
  members: Member[] | undefined = [];
  Predicate = 'Liked';
  pageNumber: number = 1;
  pageSize: number = 5;
  pagination: Pagination | undefined;
  // userParam: UserParams | UserParams;

  constructor(private memberService: MemberService) {}

  ngOnInit() {
    this.getLikes();
  }

  getLikes() {
    this.memberService
      .getLike(this.Predicate, this.pageNumber, this.pageSize)
      .subscribe((Likes) => {
        if (Likes) {
          console.log(Likes);
          this.members = Likes.result;
          this.pagination = Likes.Pagination;
        }
      });
  }

  pageChanged(event: any) {
    if (this.pageNumber !== event.page) {
      this.pageNumber = event.page;
      this.getLikes();
    }
  }
}
