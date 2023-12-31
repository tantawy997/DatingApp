import {
  ChangeDetectionStrategy,
  Component,
  Input,
  OnInit,
  ViewChild,
} from '@angular/core';
import { Message } from 'src/app/Models/message';
import { faClock } from '@fortawesome/free-solid-svg-icons';
import { MessageService } from 'src/app/Services/message.service';
import { NgForm } from '@angular/forms';

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,
  selector: 'app-member-messages',
  templateUrl: './member-messages.component.html',
  styleUrls: ['./member-messages.component.css'],
})
export class MemberMessagesComponent implements OnInit {
  //messages: Message[] = [];
  faClock = faClock;
  flag: boolean = false;
  @Input() userName: string = '';
  //@Input() messages: Message[] = [];
  messageContent: string = '';
  @ViewChild('formChat') formChat?: NgForm;

  constructor(public MessagesService: MessageService) {}

  ngOnInit() {
    // this.loadMessages();
  }

  sendMessage() {
    this.flag = true;
    this.MessagesService.SendMessage(this.messageContent, this.userName)
      .then(() => {
        this.formChat?.resetForm();
      })
      .finally(() => (this.flag = false));
  }
}
