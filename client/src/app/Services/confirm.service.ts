import { Injectable } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ConfirmDialogComponent } from '../modals/confirm-dialog/confirm-dialog.component';
import { Observable, map } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ConfirmService {
  bsModelRef?: BsModalRef<ConfirmDialogComponent>;
  constructor(private modelService: BsModalService) {}

  confirm(
    title = 'title',
    message = 'are you sure you still haven not saved your changes',
    btnOkText = 'ok',
    btnCancelText = 'cancel'
  ): Observable<boolean> {
    const config = {
      initialState: {
        title,
        message,
        btnOkText,
        btnCancelText,
      },
    };
    this.bsModelRef = this.modelService.show(ConfirmDialogComponent, config);

    return this.bsModelRef.onHidden!.pipe(
      map(() => {
        return this.bsModelRef!.content!.result;
      })
    );
  }
}
