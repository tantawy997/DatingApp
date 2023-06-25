import { Component, Input, OnInit } from '@angular/core';
import { Member } from 'src/app/Models/member';
import { faTrash, faUpload, faBan } from '@fortawesome/free-solid-svg-icons';
import { FileUploader } from 'ng2-file-upload';
import { environment } from 'src/environments/environment';
import { User } from 'src/app/Models/user';
import { AccountService } from 'src/app/Services/Account.service';
import { MemberService } from 'src/app/Services/member.service';
import { Photo } from 'src/app/Models/photo';
import { take } from 'rxjs';
@Component({
  selector: 'app-photo-editor',
  templateUrl: './photo-editor.component.html',
  styleUrls: ['./photo-editor.component.css'],
})
export class PhotoEditorComponent implements OnInit {
  faTrash = faTrash;
  faUpload = faUpload;
  faBan = faBan;
  @Input() member: Member | undefined;
  uploader: FileUploader | undefined;
  hasBaseDropZoneOver = false;
  baseUrl = environment.BaseApi;
  user: User | undefined;
  /**
   *
   */
  constructor(
    private AccountService: AccountService,
    private memberService: MemberService
  ) {
    this.user = this.AccountService.user;
  }
  ngOnInit() {
    this.initializeUploader();
  }

  fileOverBase(e: any) {
    this.hasBaseDropZoneOver = e;
  }

  initializeUploader() {
    this.uploader = new FileUploader({
      url: this.baseUrl + 'Users/Add-Photo',
      isHTML5: true,
      maxFileSize: 10 * 10024 * 1024,
      allowedFileType: ['image'],
      removeAfterUpload: true,
      autoUpload: false,
      authToken: 'bearer ' + this.user?.token,
    });
    this.uploader.onAfterAddingFile = (file) => {
      file.withCredentials = false;
    };

    this.uploader.onSuccessItem = (item, response, status, headers) => {
      if (response) {
        const photo = JSON.parse(response);
        this.member?.photos.push(photo);
      }
    };
  }

  deletePhoto(photoId: string) {
    this.memberService.deletePhoto(photoId).subscribe(() => {
      if (this.member) {
        this.member.photos = this.member.photos.filter(
          (x) => x.photoId !== photoId
        );
      }
    });
  }

  SetMainPhoto(Photo: Photo) {
    if (this.user == null) return;

    this.user.photoUrl = Photo.url;
    this.memberService.SetMainPhoto(Photo).subscribe(() => {
      if (this.user && this.member) {
        this.user.photoUrl = Photo.url;
        this.AccountService.setCurrentUser(this.user);
        this.member.photoUrl = Photo.url;
        this.member.photos.forEach((p) => {
          if (p.isMain) p.isMain = false;
          if (p.photoId == Photo.photoId) Photo.isMain = true;
        });
      }
    });
  }
}
