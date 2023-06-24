import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { MatMenuModule } from '@angular/material/menu';
import { MatButtonModule } from '@angular/material/button';
import { HomeComponent } from './home/home.component';
import { RegisterComponent } from '../components/register/register.component';
import { HeaderComponent } from './header/header.component';

@NgModule({
  declarations: [HomeComponent, RegisterComponent, HeaderComponent],
  imports: [
    CommonModule,
    RouterModule,
    FormsModule,
    MatMenuModule,
    MatButtonModule,
  ],
  exports: [HomeComponent, HeaderComponent, RouterModule],
})
export class SharedModule {}
