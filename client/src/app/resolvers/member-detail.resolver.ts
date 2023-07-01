import { inject } from '@angular/core';
import { ResolveFn } from '@angular/router';
import { MemberService } from '../Services/member.service';

export const memberDetailResolver: ResolveFn<boolean> = (route, state) => {
  ///
  return inject(MemberService).GetMember(route.paramMap.get('userName')!);
  //return true;
};
