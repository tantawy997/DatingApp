import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { Member } from 'src/app/Models/member';
import { User } from 'src/app/Models/user';
import { MemberService } from 'src/app/Services/member.service';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css'],
})
export class MemberListComponent implements OnInit {
  //members: Member[] = [];
  members$: Observable<Member[]> | undefined;

  constructor(private MemberService: MemberService) {}
  ngOnInit() {
    // this.MemberService.GetMembers().subscribe((Response) => {
    //   this.members = Response;
    //   console.log(this.members);
    // });
    this.members$ = this.MemberService.GetMembers();
  }
  identify(index: number, item: Member) {
    return item.userId;
  }
}
