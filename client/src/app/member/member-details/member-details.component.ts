import {
  Component,
  OnInit,
  ViewChild,
  AfterViewInit,
  ChangeDetectorRef,
  OnDestroy,
} from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { RouteReuseStrategy } from '@angular/router';

import {
  NgxGalleryAnimation,
  NgxGalleryImage,
  NgxGalleryOptions,
} from '@kolkov/ngx-gallery';
import { take } from 'rxjs';
import { Member } from 'src/app/Models/member';
import { User } from 'src/app/Models/user';
import { AccountService } from 'src/app/Services/Account.service';
import { TabDirective, TabsetComponent } from 'ngx-bootstrap/tabs';
import { MessageService } from 'src/app/Services/message.service';
import { Message } from 'src/app/Models/message';
import { PresenceService } from 'src/app/Services/presence.service';
import { faUserCircle } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-member-details',
  templateUrl: './member-details.component.html',
  styleUrls: ['./member-details.component.css'],
})
export class MemberDetailsComponent
  implements OnInit, AfterViewInit, OnDestroy
{
  @ViewChild('memberTabs', { static: true }) memberTabs?: TabsetComponent;
  faUser = faUserCircle;
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
    private route: RouteReuseStrategy,

    private accountService: AccountService,
    private AccountService: AccountService,
    private MessagesService: MessageService,
    private _cdr: ChangeDetectorRef,
    public presenceService: PresenceService
  ) {
    this.AccountService.CurrentUser$.pipe(take(1)).subscribe((response) => {
      if (response) this.user = response;
    });
    this.route.shouldReuseRoute = () => false;
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
      this.MessagesService.CreateHubConnection(this.user, this.member.userName);
    } else {
      this.MessagesService.stopHubConnection();
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
  ngOnDestroy(): void {
    this.MessagesService.stopHubConnection();
  }
}
