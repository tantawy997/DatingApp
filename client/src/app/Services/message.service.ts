import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Message } from '../Models/message';
import { GetPaginationHeader, getPaginationResults } from './PaginationHelper';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { User } from '../Models/user';
import { BehaviorSubject, take } from 'rxjs';
import { Group } from '../Models/group';
import { BusyService } from './busy.service';

@Injectable({
  providedIn: 'root',
})
export class MessageService {
  baseUrl: string = environment.BaseApi;
  HubUrl = environment.HubUrl;
  public hubConnection?: HubConnection;
  messagesThreadSubject: BehaviorSubject<Message[]> = new BehaviorSubject<
    Message[]
  >([]);
  messagesThread$ = this.messagesThreadSubject.asObservable();

  constructor(private http: HttpClient, private busyService: BusyService) {}

  async SendMessage(content: string, username: string) {
    return this.hubConnection
      ?.invoke('SendMessage', {
        recipientUserName: username,
        content,
      })
      .catch((error) => console.log(error));
  }

  CreateHubConnection(user: User, otherUserUserName: string) {
    this.busyService.busy();
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(this.HubUrl + 'message?user=' + otherUserUserName, {
        accessTokenFactory: () => user.token,
      })
      .withAutomaticReconnect()
      .build();

    this.hubConnection
      .start()
      .catch((error) => console.log(error))
      .finally(() => this.busyService.idle());

    this.hubConnection.on('receiveMessageThread', (messages) => {
      this.messagesThreadSubject.next(messages);
    });

    this.hubConnection.on('UpdatedGroup', (group: Group) => {
      if (group.Connections.some((x) => x.userName === otherUserUserName)) {
        this.messagesThread$.pipe(take(1)).subscribe((messages) => {
          messages.forEach((message) => {
            if (!message.messageReadDate) {
              message.messageReadDate = new Date(Date.now());
            }
          });
          this.messagesThreadSubject.next([...messages]);
        });
      }
    });
    this.hubConnection?.on('NewMessage', (message) => {
      this.messagesThread$.pipe(take(1)).subscribe((messages) => {
        this.messagesThreadSubject.next([...messages, message]);
      });
    });
  }
  stopHubConnection() {
    if (this.hubConnection) {
      this.messagesThreadSubject.next([]);
      this.hubConnection.stop();
    }
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

  deleteMessage(id: number) {
    return this.http.delete(this.baseUrl + 'Message/' + id);
  }
}
