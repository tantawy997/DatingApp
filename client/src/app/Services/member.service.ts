import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Member } from '../Models/member';

@Injectable({
  providedIn: 'root',
})
export class MemberService {
  baseUrl: string;
  constructor(private http: HttpClient) {
    this.baseUrl = environment.BaseApi;
  }

  GetMembers() {
    return this.http.get<Member[]>(this.baseUrl + 'Users/GetUsers');
  }

  GetMember(userName: string) {
    return this.http.get<Member>(this.baseUrl + 'Users/' + userName);
  }

  getHttpToken() {
    const tokenJson = localStorage.getItem('user');
    if (!tokenJson) return;

    const jsonParse = JSON.parse(tokenJson);

    return jsonParse.token;
  }
}
