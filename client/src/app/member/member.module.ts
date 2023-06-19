import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MemberListComponent } from './member-list/member-list.component';
import { MemberDetailsComponent } from './member-details/member-details.component';
import { RouterModule } from '@angular/router';
import { memberRoutingModule } from './member-routing.module';

@NgModule({
  declarations: [MemberListComponent, MemberDetailsComponent],
  imports: [CommonModule, RouterModule, memberRoutingModule],
  exports: [RouterModule],
})
export class MemberModule {}
