import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MemberListComponent } from './member-list/member-list.component';
import { MemberDetailsComponent } from './member-details/member-details.component';



@NgModule({
  declarations: [
    MemberListComponent,
    MemberDetailsComponent
  ],
  imports: [
    CommonModule
  ]
})
export class MemberModule { }
