import { Injectable } from '@angular/core';
import {
  HttpTransportType,
  HubConnection,
  HubConnectionBuilder,
} from '@microsoft/signalr';
import { ToastrService } from 'ngx-toastr';
import { environment } from 'src/environments/environment';
import { User } from '../Models/user';
import * as signalR from '@microsoft/signalr';
import { BehaviorSubject, take } from 'rxjs';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root',
})
export class PresenceService {
  hubUrl = environment.HubUrl;
  private hubConnection?: HubConnection;
  onLineUsersSubject = new BehaviorSubject<string[]>([]);
  onLineUsers$ = this.onLineUsersSubject.asObservable();

  constructor(private toaster: ToastrService, private router: Router) {}

  createHubConnection(user: User) {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(this.hubUrl + 'presence', {
        accessTokenFactory: () => user.token,
      })
      .withAutomaticReconnect()

      .build();

    this.hubConnection.start().catch((error) => console.log(error));

    this.hubConnection.on('UserIsOnLine', (username) => {
      this.onLineUsers$.pipe(take(1)).subscribe((usernames) => {
        this.onLineUsersSubject.next([...usernames, username]);
      });
    });

    this.hubConnection.on('UserIsOffLine', (username) => {
      this.onLineUsers$.pipe(take(1)).subscribe((usernames) => {
        this.onLineUsersSubject.next(usernames.filter((x) => x !== username));
      });
    });

    this.hubConnection.on('GetOnLineUsers', (usernames) => {
      console.log(usernames);
      this.onLineUsersSubject.next(usernames);
    });
    this.hubConnection.on('NewMessageRecieved', ({ username, knownAs }) => {
      this.toaster
        .info(knownAs + ' has sent you a massage click to see it')
        .onTap.pipe(take(1))
        .subscribe(() => {
          this.router.navigateByUrl(
            '/members/MembersList/' + username + '?tab=Messages'
          );
        });
    });
  }

  stopHubConnection() {
    this.hubConnection?.stop().catch((error) => console.log(error));
  }
}
