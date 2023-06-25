import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Member } from '../Models/member';
import { map, of } from 'rxjs';
import { Photo } from '../Models/photo';

@Injectable({
  providedIn: 'root',
})
export class MemberService {
  baseUrl: string;
  members: Member[] = [];
  constructor(private http: HttpClient) {
    this.baseUrl = environment.BaseApi;
  }

  GetMembers() {
    if (this.members.length > 0) return of(this.members);

    return this.http.get<Member[]>(this.baseUrl + 'Users/GetUsers').pipe(
      map((members) => {
        this.members = members;
        return members;
      })
    );
  }

  GetMember(userName: string) {
    const member = this.members.find((x) => x.userName == userName);
    if (member) return of(member);

    return this.http.get<Member>(this.baseUrl + 'Users/' + userName);
  }

  getHttpToken() {
    const tokenJson = localStorage.getItem('user');
    if (!tokenJson) return;

    const jsonParse = JSON.parse(tokenJson);

    return jsonParse.token;
  }

  UpdateMember(member: Member) {
    return this.http.put(this.baseUrl + 'Users', member).pipe(
      map(() => {
        const index = this.members.indexOf(member);
        this.members[index] = { ...this.members[index], ...member };
      })
    );
  }

  SetMainPhoto(Photo: Photo) {
    return this.http.put(
      this.baseUrl + 'Users/set-main-photo/' + Photo.photoId,
      {}
    );
  }

  deletePhoto(photoId: string) {
    return this.http.delete(this.baseUrl + 'Users/delete-photo/' + photoId);
  }
}
