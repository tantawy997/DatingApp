import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import {
  NgxGalleryAnimation,
  NgxGalleryImage,
  NgxGalleryOptions,
} from '@kolkov/ngx-gallery';
import { Member } from 'src/app/Models/member';
import { MemberService } from 'src/app/Services/member.service';

@Component({
  selector: 'app-member-details',
  templateUrl: './member-details.component.html',
  styleUrls: ['./member-details.component.css'],
})
export class MemberDetailsComponent implements OnInit {
  member: Member = {} as Member;
  UserName!: string;
  galleryOptions: NgxGalleryOptions[] = [];
  galleryImages: NgxGalleryImage[] = [];
  constructor(
    private ActiveRoute: ActivatedRoute,
    private router: Router,
    private MemberService: MemberService
  ) {}
  ngOnInit() {
    let photos: any = [];

    this.ActiveRoute.paramMap.subscribe((res) => {
      this.UserName = res.get('userName') as string;
      console.log(this.UserName);
      this.MemberService.GetMember(this.UserName).subscribe((response) => {
        this.member = response;
        for (const photo of this.member.photos) {
          photos.push({
            small: photo.url,
            medium: photo.url,
            big: photo.url,
          });
        }
        this.galleryImages = photos;
      });
    });

    this.galleryOptions = [
      {
        width: '200px',
        height: '200px',
        imagePercent: 100,
        thumbnailsColumns: 4,
        imageAnimation: NgxGalleryAnimation.Slide,
        preview: false,
      },
    ];
  }
}
