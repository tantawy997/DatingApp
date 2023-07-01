import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Member } from '../Models/member';
import { map, observable, of, take } from 'rxjs';
import { Photo } from '../Models/photo';
import { PaginationResult } from '../Models/pagination';
import { UserParams } from '../Models/user-params';
import { AccountService } from './Account.service';
import { User } from '../Models/user';
import { GetPaginationHeader, getPaginationResults } from './PaginationHelper';

@Injectable({
  providedIn: 'root',
})
export class MemberService {
  baseUrl: string;
  members: Member[] = [];
  user: User | undefined;
  userParam: UserParams | undefined;
  MemberCache = new Map();
  constructor(
    private http: HttpClient,
    private accountService: AccountService
  ) {
    this.baseUrl = environment.BaseApi;
    this.accountService.CurrentUser$.pipe(take(1)).subscribe((user) => {
      if (user) {
        this.user = user;
        this.userParam = new UserParams(this.user);
      }
    });
  }
  getUserParams() {
    return this.userParam;
  }
  setUserParams(Param: UserParams) {
    this.userParam = Param;
  }
  ResetUSerParams() {
    if (this.user) {
      this.userParam = new UserParams(this.user);
    }
    return this.userParam;
  }
  GetMembers(userParams: UserParams) {
    //if (this.members.length > 0) return of(this.members);
    var response = this.MemberCache.get(Object.values(userParams).join('-'));
    if (response) {
      return of(response);
    }

    let params = GetPaginationHeader(
      userParams.pageNumber,
      userParams.pageSize
    );

    params = params.append('gender', userParams.gender);
    params = params.append('maxAge', userParams.maxAge);
    params = params.append('minAge', userParams.minAge);
    params = params.append('orderBy', userParams.orderBy);

    return getPaginationResults<Member[]>(
      this.baseUrl + 'Users/GetUsers',
      params,
      this.http
    ).pipe(
      map((res) => {
        this.MemberCache.set(Object.values(userParams).join('-'), res);
        return res;
      })
    );
  }

  GetMember(userName: string) {
    let resp = [...this.MemberCache.values()]
      .reduce((arr, elem) => arr.concat(elem.result), [])
      .find((x: Member) => x.userName == userName);
    if (resp) return of(resp);

    return this.http.get<Member>(this.baseUrl + 'Users/' + userName);
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

  addLike(username: string) {
    return this.http.post(this.baseUrl + 'Like/' + username, {});
  }

  getLike(Predicate: string, pageNumber: number, pageSize: number) {
    var params = GetPaginationHeader(pageNumber, pageSize);
    params = params.append('Predicate', Predicate);
    return getPaginationResults<Member[]>(
      this.baseUrl + 'Like',
      params,
      this.http
    );
  }
  getHttpToken() {
    const tokenJson = localStorage.getItem('user');
    if (!tokenJson) return;

    const jsonParse = JSON.parse(tokenJson);

    return jsonParse.token;
  }
}
