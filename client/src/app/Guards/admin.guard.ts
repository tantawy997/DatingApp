import { inject } from '@angular/core';
import { CanActivateFn } from '@angular/router';
import { AccountService } from '../Services/Account.service';
import { map } from 'rxjs';
import { ToastrService } from 'ngx-toastr';

export const adminGuard: CanActivateFn = (route, state) => {
  var toaster: ToastrService = {} as ToastrService;
  return inject(AccountService).CurrentUser$.pipe(
    map((user) => {
      if (!user) return false;
      console.log(user);

      if (user.roles.includes('Admin') || user.roles.includes('Moderator')) {
        return true;
      } else {
        //console.log()

        return false;
      }
    })
  );
};
