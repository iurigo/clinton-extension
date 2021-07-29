// Angular
import { Component, OnInit } from '@angular/core';
// Components
import { DialogConfirmService } from 'src/app/shared/components/dialog-confirm/dialog-confirm.service';
import { DialogFormService } from 'src/app/shared/components/dialog-form/dialog-form.service';
// Services
import { NotifyService } from 'src/app/services/notify.service';
import { UsersService } from './users.service';
// Interfaces
import { ActionItem } from 'src/app/shared/components/menu-button/menu-button.interfaces';
import { User, UserDialog, UserNew, UserUpdate } from './users.interface';
import { FormItem, FormItemBoolean, FormItemPassword, FormItemText } from 'src/app/shared/components/dynamic-form/dynamic-form.interfaces';
import { FormItemType } from 'src/app/shared/components/dynamic-form/dynamic-form.enums';
import { isEqualToValidator } from 'src/app/shared/validators/is-equal-to.validator';
import { Role, Status } from './users.enum';
// Consts
const MENU_UPDATE = 'update';
const MENU_RESET_PASSWORD = 'reset-password';
const MENU_DELETE = 'delete';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.scss']
})
export class UsersComponent implements OnInit {

  public users: User[] = [];
  public userActions: ActionItem[] = [];

  public isBusy: boolean = true;
  public isMicroBusy: boolean = false;

  constructor(
    private _notify: NotifyService,
    private _service: UsersService,
    private _dialogForm: DialogFormService,
    private _dialogConfirm: DialogConfirmService
  ) { }

  public async ngOnInit() {
    this.userActions = this.getGridActions();

    // Get the list of users
    await this.loadUsers();
    this.isBusy = false;
  }

  private async loadUsers(): Promise<void> {
    try {
      this.users = await this._service.getAllUsers().toPromise();
    } catch { }
  }

  public async onUserCreate(): Promise<void> {
    const dialogItems = this.getUserDialogItems(true);
    dialogItems.find(i => i.field === 'isActive').value = true;

    const dialogResult = await this._dialogForm.show<UserDialog>(
      "Create a new User",
      dialogItems,
      { actionName: 'Create' }
    );

    if (dialogResult.positive) {
      // Create a new user
      try {
        this.isBusy = true;
        // Create an new user model
        const model = dialogResult.model;
        const newUser: UserNew = {
          username: model.username,
          fullName: model.fullName,
          role: model.isAdmin ? Role[Role.ADMIN] : Role[Role.USER],
          status: model.isActive ? Status[Status.ACTIVE] : Status[Status.INACTIVE],
          password: model.password
        };
        await this._service.createUser(newUser).toPromise();
        // Load users
        await this.loadUsers();
        this._notify.success("The user was successfully created.");
      } finally {
        this.isBusy = false;
      }
    }
  }

  public async onUserUpdate(userId: number): Promise<void> {

    const user = this.users.find(p => p.id === userId);
    const dialogItems = this.getUserDialogItems();
    dialogItems.find(i => i.field === 'fullName').value = user.fullName;
    dialogItems.find(i => i.field === 'username').value = user.username;
    dialogItems.find(i => i.field === 'isAdmin').value = user.isAdmin;
    dialogItems.find(i => i.field === 'isActive').value = user.isActive;

    const dialogResult = await this._dialogForm.show<UserDialog>(
      "Update User",
      dialogItems,
      { actionName: 'Update' }
    );

    // Create an update user model
    const model = dialogResult.model;
    const existingUser: UserUpdate = {
      id: user.id,
      username: model.username,
      fullName: model.fullName,
      role: model.isAdmin ? Role[Role.ADMIN] : Role[Role.USER],
      status: model.isActive ? Status[Status.ACTIVE] : Status[Status.INACTIVE]
    };

    if (dialogResult.positive) {
      // Update the user
      try {
        this.isBusy = true;
        await this._service.updateUser(existingUser).toPromise();
        // Load users
        await this.loadUsers();
        this._notify.success("The user was successfully updated.");
      } finally {
        this.isBusy = false;
      }
    }
  }

  public async onUserDelete(userId: number): Promise<void> {
    const user = this.users.find(p => p.id === userId);
    const ok = await this._dialogConfirm.show(
      `Delete "${user.fullName}" User`,
      "Are you sure?"
    );
    if (ok) {
      // Delete the user
      try {
        this.isBusy = true;
        await this._service.deleteUser(userId).toPromise();
        // Load users
        await this.loadUsers();
        this._notify.success('The user was successfully deleted');
      } finally {
        this.isBusy = false;
      }
    }
  }

  public async resetPassword(userId: number): Promise<void> {
    const dialog = await this._dialogForm.show<{ password: string }>('Reset password', [
      { type: FormItemType.PASSWORD, field: 'password', title: 'New password', isRequired: true, focus: true }
    ], { actionName: 'Reset' });

    if (dialog.positive) {
      await this._service.resetPassword(userId, dialog.model.password).toPromise();
      this._notify.success('The password was successfully updated.');
    }
  }

  public async onUserAction(e, id: number): Promise<void> {
    switch (e.id) {
      case MENU_UPDATE: await this.onUserUpdate(id); break;
      case MENU_RESET_PASSWORD: await this.resetPassword(id); break;
      case MENU_DELETE: await this.onUserDelete(id); break;
    }
  }

  // [ Helper functions ]
  private getUserDialogItems(withPassword?: boolean): FormItem[] {
    const formItems: FormItem[] = [
      <FormItemText>{ type: FormItemType.TEXT, field: 'username', title: 'Username', isRequired: true, focus: true },
      <FormItemText>{ type: FormItemType.TEXT, field: 'fullName', title: 'Full Name', isRequired: true },
      <FormItemBoolean>{ type: FormItemType.BOOLEAN, field: 'isAdmin', title: 'isAdmin' },
      <FormItemBoolean>{ type: FormItemType.BOOLEAN, field: 'isActive', title: 'isActive' },
    ];

    if (!!withPassword) {
      formItems.splice(1, 0, <FormItemPassword>{ type: FormItemType.PASSWORD, field: 'password', title: 'Password', minLength: 3, isRequired: true, validators: [isEqualToValidator('confirm', true)] });
      formItems.splice(2, 0, <FormItemPassword>{ type: FormItemType.PASSWORD, field: 'confirm', title: 'Confirm Password', isRequired: true, validators: [isEqualToValidator('password', false)] });
    }

    return formItems;
  }

  private getGridActions(): ActionItem[] {
    return [
      { id: MENU_UPDATE, icon: "k-icon k-i-edit", name: "Update" },
      { id: MENU_RESET_PASSWORD, icon: 'k-icon k-i-reset', name: 'Reset Password' },
      { id: MENU_DELETE, icon: "k-icon k-i-delete", name: "Delete" }
    ];
  }
}
