import { Component, OnInit } from '@angular/core';
import { BsModalRef, BsModalService, ModalOptions } from 'ngx-bootstrap/modal';
import { RolesModalComponent } from 'src/app/Modals/roles-modal/roles-modal.component';
import { User } from 'src/app/Models/user';
import { AdminService } from 'src/app/Services/admin.service';

@Component({
  selector: 'app-user-management',
  templateUrl: './user-management.component.html',
  styleUrls: ['./user-management.component.css'],
})
export class UserManagementComponent implements OnInit {
  users: User[] = [];
  modalref: BsModalRef<RolesModalComponent> =
    new BsModalRef<RolesModalComponent>();
  availableRoles = ['Admin', 'Member', 'Moderator'];
  constructor(
    private AdminService: AdminService,
    private bsModalService: BsModalService
  ) {}
  ngOnInit(): void {
    this.getUsersWithRoles();
  }

  getUsersWithRoles() {
    this.AdminService.GetUsersWithRoles().subscribe((users) => {
      this.users = users;
    });
  }

  openRolesModal(user: User) {
    const config = {
      class: 'modal-dialog-centered',
      initialState: {
        userName: user.userName,
        userRoles: this.availableRoles,
        selectedRoles: [...user.roles],
      },
    };
    this.modalref = this.bsModalService.show(RolesModalComponent, config);
    this.modalref.onHide?.subscribe(() => {
      const selectedRoles = this.modalref.content?.selectedRoles;
      if (!this.arrayEqual(selectedRoles!, user.roles)) {
        this.AdminService.updateUserRoles(
          user.userName,
          selectedRoles!
        ).subscribe((roles) => {
          user.roles = roles;
        });
      }
    });
  }

  arrayEqual(arr1: any[], arr2: any[]) {
    return JSON.stringify(arr1.sort()) === JSON.stringify(arr2.sort());
  }
}
