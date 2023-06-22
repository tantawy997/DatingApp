import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MemberListComponent } from './member-list/member-list.component';
import { MemberDetailsComponent } from './member-details/member-details.component';
import { RouterModule } from '@angular/router';
import { memberRoutingModule } from './member-routing.module';
import { FormsModule } from '@angular/forms';
import { MemberCardComponent } from './member-card/member-card.component';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { MatTabsModule } from '@angular/material/tabs';
import { NgxGalleryModule } from '@kolkov/ngx-gallery';

@NgModule({
  declarations: [
    MemberListComponent,
    MemberDetailsComponent,
    MemberCardComponent,
  ],
  imports: [
    CommonModule,
    RouterModule,
    memberRoutingModule,
    FormsModule,
    FontAwesomeModule,
    MatTabsModule,
    NgxGalleryModule,
  ],
  exports: [RouterModule],
})
export class MemberModule {}
