import { Component, Input } from '@angular/core';
import { Member } from 'src/app/Models/member';
import { faUser } from '@fortawesome/free-solid-svg-icons';
import { faHeart, faEnvelope } from '@fortawesome/free-regular-svg-icons';
import { Router } from '@angular/router';
import { MemberService } from 'src/app/Services/member.service';
import { ToastrService } from 'ngx-toastr';
import { PresenceService } from 'src/app/Services/presence.service';

@Component({
  selector: 'app-member-card',
  templateUrl: './member-card.component.html',
  styleUrls: ['./member-card.component.css'],
})
export class MemberCardComponent {
  faUser = faUser;
  faHeart = faHeart;
  faEnvelope = faEnvelope;
  @Input() member: Member = {} as Member;
  constructor(
    private router: Router,
    private memberService: MemberService,
    private toaster: ToastrService,
    public PresenceService: PresenceService
  ) {}
  toChild(username: string) {
    this.router.navigateByUrl('members/MembersList/' + username);
  }

  addLike(member: Member) {
    this.memberService.addLike(member.userName).subscribe((res) => {
      this.toaster.success('you have liked ' + member.knownAs);
    });
  }
}
