import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MemberListComponent } from './member-list/member-list.component';
import { MemberDetailsComponent } from './member-details/member-details.component';
import { authGuard } from '../Guards/auth.guard';
import { EditComponent } from './edit/edit.component';
import { preventBeforeSaveGuard } from '../Guards/prevent-before-save.guard';
import { memberDetailResolver } from '../resolvers/member-detail.resolver';

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
    resolve: { member: memberDetailResolver },
  },
  {
    path: 'member/edit',
    component: EditComponent,
    canActivate: [authGuard],
    canDeactivate: [preventBeforeSaveGuard],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class memberRoutingModule {}
