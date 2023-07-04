import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatMenuModule } from '@angular/material/menu';
import { MatButtonModule } from '@angular/material/button';
import { HomeComponent } from './home/home.component';
import { RegisterComponent } from '../components/register/register.component';
import { HeaderComponent } from './header/header.component';
import { TextInputComponent } from '../components/text-input/text-input.component';
import { DatePickerComponent } from '../components/date-picker/date-picker.component';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatNativeDateModule } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { HasRoleDirective } from '../directives/has-role.directive';
import { ModalModule } from 'ngx-bootstrap/modal';

@NgModule({
  declarations: [
    HomeComponent,
    RegisterComponent,
    HeaderComponent,
    TextInputComponent,
    DatePickerComponent,
    HasRoleDirective,
  ],
  imports: [
    CommonModule,
    RouterModule,
    FormsModule,
    MatMenuModule,
    MatButtonModule,
    ReactiveFormsModule,
    MatInputModule,
    MatFormFieldModule,
    MatNativeDateModule,
    MatDatepickerModule,
    FontAwesomeModule,
    ModalModule.forRoot(),
  ],

  exports: [
    HomeComponent,
    HeaderComponent,
    RouterModule,
    FontAwesomeModule,
    ModalModule,
    HasRoleDirective,
  ],
})
export class SharedModule {}
