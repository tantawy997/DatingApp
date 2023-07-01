import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Message } from '../Models/message';
import { GetPaginationHeader, getPaginationResults } from './PaginationHelper';

@Injectable({
  providedIn: 'root',
})
export class MessageService {
  baseUrl: string = environment.BaseApi;

  constructor(private http: HttpClient) {}

  SendMessage(content: string, username: string) {
    return this.http.post<Message>(this.baseUrl + 'Message', {
      recipientUserName: username,
      content,
    });
  }

  GetMessages(pageNumber: number, pageSize: number, container: string) {
    let params = GetPaginationHeader(pageNumber, pageSize);

    params = params.append('Container', container);

    return getPaginationResults<Message[]>(
      this.baseUrl + 'Message',
      params,
      this.http
    );
  }

  GetMessageThread(username: string) {
    console.log(username);

    return this.http.get<Message[]>(
      this.baseUrl + 'Message/thread/' + username
    );
  }

  deleteMessage(id: string) {
    return this.http.delete(this.baseUrl + 'Message/' + id);
  }
}
