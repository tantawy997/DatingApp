import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment.development';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  private BaseApi: string;
  constructor() {
    this.BaseApi = environment.BaseApi;
  }
}
