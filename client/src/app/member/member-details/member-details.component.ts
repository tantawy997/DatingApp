import {
  Component,
  OnInit,
  ViewChild,
  AfterViewInit,
  ChangeDetectorRef,
} from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import {
  NgxGalleryAnimation,
  NgxGalleryImage,
  NgxGalleryOptions,
} from '@kolkov/ngx-gallery';
import { take } from 'rxjs';
import { faArrowAltCircleRight } from '@fortawesome/free-regular-svg-icons';
import { Member } from 'src/app/Models/member';
import { User } from 'src/app/Models/user';
import { AccountService } from 'src/app/Services/Account.service';
import { MemberService } from 'src/app/Services/member.service';
import { TabDirective, TabsetComponent } from 'ngx-bootstrap/tabs';
import { MessageService } from 'src/app/Services/message.service';
import { Message } from 'src/app/Models/message';

@Component({
  selector: 'app-member-details',
  templateUrl: './member-details.component.html',
  styleUrls: ['./member-details.component.css'],
})
export class MemberDetailsComponent implements OnInit, AfterViewInit {
  @ViewChild('memberTabs', { static: true }) memberTabs?: TabsetComponent;

  member: Member = {} as Member;
  UserName!: string;
  user: User = {} as User;
  galleryOptions: NgxGalleryOptions[] = [];
  messages: Message[] = [];
  galleryImages: NgxGalleryImage[] = [];
  activeTab: TabDirective = {} as TabDirective;
  constructor(
    private ActiveRoute: ActivatedRoute,
    private router: Router,
    private MemberService: MemberService,
    private AccountService: AccountService,
    private MessagesService: MessageService,
    private _cdr: ChangeDetectorRef
  ) {
    this.AccountService.CurrentUser$.pipe(take(1)).subscribe((response) => {
      if (response) this.user = response;
    });
  }
  ngOnInit() {
    this.ActiveRoute.data.subscribe((data) => {
      this.member = data['member'];
    });
    this.ActiveRoute.queryParams.subscribe((params) => {
      if (params) {
        this.selectTab(params['tab']);
      }
    });

    this.galleryOptions = [
      {
        width: '200px',
        height: '200px',
        imagePercent: 100,
        thumbnailsColumns: 6,
        imageAnimation: NgxGalleryAnimation.Slide,
        preview: false,
        imageArrows: true,
        arrowPrevIcon: 'fa fa-arrow-circle-left',
        arrowNextIcon: 'fa fa-arrow-circle-right',
      },
    ];
    this.galleryImages = this.getImages();
  }

  getImages() {
    let photos: any = [];

    for (const photo of this.member.photos) {
      photos.push({
        small: photo.url,
        medium: photo.url,
        big: photo.url,
      });
    }
    return photos;
  }
  onActivated(data: TabDirective) {
    this.activeTab = data;
    if (this.activeTab.heading == 'Messages') {
      this.loadMessages();
    }
  }
  loadMessages() {
    console.log(this.messages);
    this.MessagesService.GetMessageThread(this.member.userName).subscribe(
      (messages) => {
        console.log(messages);
        this.messages = messages;
      }
    );
  }

  selectTab(heading: string) {
    if (this.memberTabs) {
      this.memberTabs.tabs.find((x) => x.heading == heading)!.active = true;
    }
  }

  ngAfterViewInit() {
    this.ActiveRoute.queryParams.subscribe((params) => {
      const selectedTab = params['tab'];
      this.activeTab.selectTab = selectedTab > 0 ? selectedTab : 0;
    });
  }
}
