import { Component } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { ConfirmService } from 'src/app/Services/confirm.service';

@Component({
  selector: 'app-confirm-dialog',
  templateUrl: './confirm-dialog.component.html',
  styleUrls: ['./confirm-dialog.component.css'],
})
export class ConfirmDialogComponent {
  title = '';
  message = '';
  btnOkText = '';
  btnCancelText = '';

  result = false;

  /**
   *
   */
  constructor(
    public bsModelRef: BsModalRef,
    private confirmService: ConfirmService
  ) {}

  confirm() {
    this.result = true;
    this.bsModelRef.hide();
  }

  cancel() {
    this.bsModelRef.hide();
  }
}
