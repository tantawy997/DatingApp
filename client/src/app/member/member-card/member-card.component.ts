import { Component, Input } from '@angular/core';
import { Member } from 'src/app/Models/member';
import { faUser } from '@fortawesome/free-solid-svg-icons';
import { faHeart, faEnvelope } from '@fortawesome/free-regular-svg-icons';
import { Router } from '@angular/router';

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
  constructor(private router: Router) {}
  toChild(username: string) {
    this.router.navigateByUrl('members/MembersList/' + username);
  }
}
