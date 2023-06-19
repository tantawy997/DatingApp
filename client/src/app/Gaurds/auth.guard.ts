import { CanActivateFn, Router } from '@angular/router';
import { Injectable, inject } from '@angular/core';
import { AccountService } from '../Services/Account.service';
import { ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root',
})
class UserToken {}

export const authGuard: CanActivateFn = (route, state) => {
  if (inject(AccountService).loggedIn !== true) {
    //inject(ToastrService).error('unauthorized access');
    inject(Router).navigateByUrl('/');

    return false;
  }

  return true;
};
