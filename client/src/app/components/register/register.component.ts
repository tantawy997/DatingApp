import { Component, Output, EventEmitter, OnInit } from '@angular/core';
import {
  AbstractControl,
  FormBuilder,
  FormControl,
  FormGroup,
  ValidatorFn,
  Validators,
} from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { User } from 'src/app/Models/user';
import { AccountService } from 'src/app/Services/Account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css'],
})
export class RegisterComponent implements OnInit {
  user: User = {} as User;
  @Output() cancelRegister = new EventEmitter();
  registerForm: FormGroup = new FormGroup({});
  maxDate: Date = new Date();

  constructor(
    public AccountService: AccountService,
    private router: Router,
    private toaster: ToastrService,
    private fb: FormBuilder
  ) {}
  ngOnInit() {
    this.maxDate.setFullYear(this.maxDate.getFullYear() - 18);
    this.registerForm = this.fb.group({
      userName: ['', Validators.required],
      gender: ['male'],
      city: ['', Validators.required],
      country: ['', Validators.required],
      dateOfBirth: ['', Validators.required],
      knownAs: ['', Validators.required],

      password: [
        '',
        [
          Validators.required,
          Validators.minLength(4),
          Validators.maxLength(16),
        ],
      ],
      confirmPassword: [
        '',
        [Validators.required, this.matchValues('password')],
      ],
    });
    this.registerForm.controls['password'].valueChanges.subscribe((_) => {
      this.registerForm.controls['confirmPassword'].updateValueAndValidity();
    });
  }
  cancel() {
    this.cancelRegister.emit(false);
  }
  matchValues(matchTo: string): ValidatorFn {
    return (control: AbstractControl) => {
      return control.value === control.parent?.get(matchTo)?.value
        ? null
        : { notMatching: true };
    };
  }

  validationErrors: string[] = [];
  signUp() {
    const dot = this.GetDateOnly(
      this.registerForm.controls['dateOfBirth'].value
    );
    const values = { ...this.registerForm.value, dateOfBirth: dot };
    console.log(values);
    this.AccountService.register(values).subscribe(
      (response) => {
        //console.log(response);
        this.toaster.success('you registered successfully');
        this.router.navigateByUrl('/members/MembersList');
      },
      (e) => {
        console.log(e.message);

        //this.toaster.error(e.message);
        // this.validationErrors = e.errors;
      }
    );
  }

  private GetDateOnly(dot: string | undefined) {
    if (!dot) return;

    let dateofBirth = new Date(dot);

    return new Date(
      dateofBirth.setMinutes(
        dateofBirth.getMinutes() - dateofBirth.getTimezoneOffset()
      )
    )
      .toISOString()
      .slice(0, 10);
  }
}
