import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MemberListComponent } from './member-list/member-list.component';
import { MemberDetailsComponent } from './member-details/member-details.component';
import { authGuard } from '../Gaurds/auth.guard';

const routes: Routes = [
  {
    path: 'MembersList',
    component: MemberListComponent,
    canActivate: [authGuard],
  },
  {
    path: 'MembersList/:userName',
    component: MemberDetailsComponent,
    canActivate: [authGuard],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class memberRoutingModule {}
