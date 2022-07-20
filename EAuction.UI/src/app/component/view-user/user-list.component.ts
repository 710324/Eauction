import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { IUserService } from 'src/app/interface';
import { UserModel } from 'src/app/model';
import { UserDeleteDialogComponent } from 'src/app/component/user-delete-dialog/user-delete-dialog.component';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.scss']
})
export class UserListComponent implements OnInit {

  constructor(private userService: IUserService, public dialog: MatDialog) { }

  userList: UserModel[];
  displayedColumns: string[] = [ "fullname","address", "phone", "email", "userName", "userType", "createdDate", "updateddate", 'delete'];

  ngOnInit(): void {
    this.getAllUsers();
  }

  getAllUsers() {
    this.userService.viewAllUser().subscribe(response => {
      this.userList = response;
    });
  }

  openDialog(enterAnimationDuration: string, exitAnimationDuration: string, userId: string): void {
    this.dialog.open(UserDeleteDialogComponent, {
      width: '50%',
      enterAnimationDuration,
      exitAnimationDuration,
      data: userId
    }).afterClosed().subscribe((data: boolean) => {
      if (data) {
        this.userService.deleteUser(userId).subscribe(data => {
          if (data) {
            console.log(userId + " User deleted");
          }
          else {
            console.log(userId + " User not deleted");
          }
          this.getAllUsers();
        });
      }
    });
  }


}
