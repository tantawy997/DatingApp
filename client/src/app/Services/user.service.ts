import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment.development';
import { User } from '../Models/user';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  private baseApi: string;
  constructor(private http: HttpClient) {
    this.baseApi = environment.BaseApi;
  }

  GetAllUsers() {
    return this.http.get<User[]>(this.baseApi + 'Users/GetUsers');
  }
}
