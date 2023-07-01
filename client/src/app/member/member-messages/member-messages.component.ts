import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { Message } from 'src/app/Models/message';
import { faClock } from '@fortawesome/free-solid-svg-icons';
import { MessageService } from 'src/app/Services/message.service';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-member-messages',
  templateUrl: './member-messages.component.html',
  styleUrls: ['./member-messages.component.css'],
})
export class MemberMessagesComponent implements OnInit {
  //messages: Message[] = [];
  faClock = faClock;
  @Input() userName: string = '';
  @Input() messages: Message[] = [];
  messageContent: string = '';
  @ViewChild('formChat') formChat?: NgForm;

  constructor(private MessagesService: MessageService) {}

  ngOnInit() {
    // this.loadMessages();
  }

  sendMessage() {
    this.MessagesService.SendMessage(
      this.messageContent,
      this.userName
    ).subscribe((message) => {
      this.messages.push(message);
      this.formChat?.resetForm();
    });
  }
}
