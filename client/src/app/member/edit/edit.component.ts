import { Component, OnInit, ViewChild, HostListener } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs';
import { Member } from 'src/app/Models/member';
import { User } from 'src/app/Models/user';
import { AccountService } from 'src/app/Services/Account.service';
import { MemberService } from 'src/app/Services/member.service';

@Component({
  selector: 'app-edit',
  templateUrl: './edit.component.html',
  styleUrls: ['./edit.component.css'],
})
export class EditComponent implements OnInit {
  user: User = {} as User;
  member: Member = {} as Member;
  @HostListener('window:beforeunload', ['$event']) unloadnotifications(
    event: any
  ) {
    if (this.editForm?.dirty) {
      event.returnValue = true;
    }
  }
  @ViewChild('editForm')
  editForm: NgForm | undefined;
  constructor(
    private accountService: AccountService,
    private memberService: MemberService,
    private toaster: ToastrService
  ) {
    // this.accountService.CurrentUser$.subscribe((response) => {
    //   if (response) this.user = response;
    //   console.log(response);
    // });
  }
  ngOnInit() {
    //this.user = this.accountService.user;
    this.user = JSON.parse(localStorage.getItem('user')!);
    //console.log(this.user);
    this.memberService.GetMember(this.user.userName).subscribe((res) => {
      //console.log(res);
      this.member = res;
    });
  }

  EditUserInfo() {
    //console.log(this.editForm?.value);
    this.memberService.UpdateMember(this.editForm?.value).subscribe(
      (res) => {
        // console.log(res);
        this.toaster.success('profile updated successfully');
        this.editForm?.reset(this.member);
      },
      (e) => console.log(e)
    );
  }
}
