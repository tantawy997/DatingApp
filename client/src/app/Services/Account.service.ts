import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment.development';
import { User } from '../Models/user';
import { HttpClient } from '@angular/common/http';
import { Observable, Subject, map } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class AccountService {
  private BaseApi: string;

  loggedIn: boolean = false;
  token: string;
  userSubject: Subject<User | null> = new Subject<User | null>();
  CurrentUser: Observable<User | null> = this.userSubject.asObservable();

  constructor(private http: HttpClient) {
    this.BaseApi = environment.BaseApi;
    this.token = JSON.parse(localStorage.getItem('token')!);
    this.CurrentUser = JSON.parse(localStorage.getItem('user')!);
    if (this.token) {
      this.loggedIn = true;
    } else {
      this.loggedIn = false;
    }
  }

  login(model: User) {
    return this.http
      .post<Observable<User>>(this.BaseApi + 'Account/login', model)
      .subscribe(
        (res: any) => {
          console.log(res.token);
          this.CurrentUser = res;
          if (!localStorage.getItem('token')) {
            localStorage.setItem('token', JSON.stringify(res.token));
            localStorage.setItem('user', JSON.stringify(res));
          }

          this.userSubject.next(res);
          this.loggedIn = true;
          // console.log(this.loggedIn);
        },
        (e) => {
          console.log(e.message);
        }
      );
  }

  register(user: User) {
    return this.http
      .post<Observable<User>>(this.BaseApi + 'Account/register', user)
      .pipe(
        map((response: any) => {
          if (response) {
            this.CurrentUser = response;
            localStorage.setItem('token', JSON.stringify(response.token));
            localStorage.setItem('user', JSON.stringify(response));
            this.userSubject.next(user);
            this.loggedIn = true;

            return response;
          }
        })
      );
  }

  logout() {
    localStorage.removeItem('token');
    localStorage.removeItem('user');

    this.userSubject.next(null);
    this.loggedIn = false;
  }

  setCurrentUser(user: User) {
    this.userSubject.next(user);
  }
}
