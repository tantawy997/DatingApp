import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { User } from '../Models/user';
import { HttpClient } from '@angular/common/http';
import {
  BehaviorSubject,
  EMPTY,
  Observable,
  ReplaySubject,
  Subject,
  map,
  of,
} from 'rxjs';
import { ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root',
})
export class AccountService {
  private BaseApi: string;
  user: User = {} as User;
  loggedIn: boolean = false;
  token: string | undefined;
  private userSubject = new ReplaySubject<User>(1);
  CurrentUser$ = this.userSubject.asObservable();

  constructor(private http: HttpClient, private toaster: ToastrService) {
    this.BaseApi = environment.BaseApi;
    this.token = JSON.parse(localStorage.getItem('token')!);
    this.user = JSON.parse(localStorage.getItem('user')!);
    if (this?.token || this.user) {
      this.loggedIn = true;
    } else {
      this.loggedIn = false;
    }
  }

  login(model: User) {
    return this.http.post<User>(this.BaseApi + 'Account/login', model).pipe(
      map((user: User) => {
        if (user) {
          this.user = user;
          // console.log(user);
          localStorage.setItem('token', JSON.stringify(user.token));
          localStorage.setItem('user', JSON.stringify(user));
          this.toaster.success('welcome ' + user.userName);
          this.userSubject.next(user);

          this.loggedIn = true;
        }
        return user;
      })
    );
  }

  register(user: User) {
    return this.http.post<User>(this.BaseApi + 'Account/register', user).pipe(
      map((response: User) => {
        if (response) {
          this.user = response;
          localStorage.setItem('token', JSON.stringify(response.token));
          localStorage.setItem('user', JSON.stringify(response));
          this.userSubject.next(user);
          this.loggedIn = true;
        }
        return response;
      })
    );
  }

  logout() {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    this.CurrentUser$ = EMPTY;
    this.loggedIn = false;
    this.toaster.info('logged out successfully');
  }

  setCurrentUser(user: User) {
    user.roles = [];
    const roles = this.getDecodedToken(user.token).role;
    Array.isArray(roles) ? (user.roles = roles) : user.roles.push(roles);
    localStorage.setItem('user', JSON.stringify(user));
    this.userSubject.next(user);
  }

  getDecodedToken(token: string) {
    return JSON.parse(atob(token.split('.')[1]));
  }
}
