import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HeaderComponent } from './header/header.component';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { MatMenuModule } from '@angular/material/menu';
import { MatButtonModule } from '@angular/material/button';
import { HomeComponent } from './home/home.component';
import { RegisterComponent } from '../components/register/register.component';
import { sharedRoutingModule } from './shared-routing.module';

@NgModule({
  declarations: [HeaderComponent, HomeComponent, RegisterComponent],
  imports: [
    CommonModule,
    RouterModule,
    FormsModule,
    MatMenuModule,
    MatButtonModule,
    RouterModule,
    sharedRoutingModule,
  ],

  exports: [HeaderComponent, HomeComponent],
})
export class SharedModule {}
