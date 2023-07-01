import { Component, OnInit } from '@angular/core';
import { Message } from 'src/app/Models/message';
import { Pagination } from 'src/app/Models/pagination';
import { MessageService } from 'src/app/Services/message.service';
import {
  faEnvelope,
  faEnvelopeOpen,
  faPaperPlane,
} from '@fortawesome/free-regular-svg-icons';
import { ToastrService } from 'ngx-toastr';
@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.css'],
})
export class MessagesComponent implements OnInit {
  messages?: Message[] = [];
  pageNumber: number = 1;
  pageSize: number = 5;
  container: string = 'Inbox';
  pagination?: Pagination;
  envelope = faEnvelope;
  envelopeOpen = faEnvelopeOpen;
  PaperPlane = faPaperPlane;
  flag: boolean = false;

  constructor(
    private MessageService: MessageService,
    private toaster: ToastrService
  ) {}

  ngOnInit(): void {
    this.GetMessages();
  }

  GetMessages() {
    this.flag = true;
    this.MessageService.GetMessages(
      this.pageNumber,
      this.pageSize,
      this.container
    ).subscribe((response) => {
      this.messages = response.result;
      this.pagination = response.Pagination;
      this.flag = false;
    });
  }

  pageChanged(event: any) {
    if (event.page != this.pageNumber) {
      this.pageNumber = event.page;
      this.GetMessages();
    }
  }

  deleteMessage(id: string) {
    this.MessageService.deleteMessage(id).subscribe((response) => {
      this.messages?.splice(
        this.messages.findIndex((x) => x.id === id),
        1
      );
      ///
    });
  }
}
