import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { User } from '../Models/user';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class AdminService {
  baseUrl = environment.BaseApi;

  constructor(private http: HttpClient) {}

  GetUsersWithRoles() {
    return this.http.get<User[]>(this.baseUrl + 'Admin/Users-With-Roles');
  }

  updateUserRoles(username: string, roles: string[]) {
    return this.http.post<string[]>(
      this.baseUrl + 'Admin/edit-roles/' + username + '?Roles=' + roles,
      {}
    );
  }
}
