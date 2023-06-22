import { Component, OnInit } from '@angular/core';
import { Member } from 'src/app/Models/member';
import { MemberService } from 'src/app/Services/member.service';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css'],
})
export class MemberListComponent implements OnInit {
  members: Member[] = [];

  constructor(private MemberService: MemberService) {}
  ngOnInit() {
    this.MemberService.GetMembers().subscribe((Response) => {
      this.members = Response;
      console.log(this.members);
    });
  }
  identify(index: number, item: Member) {
    return item.userId;
  }
}
