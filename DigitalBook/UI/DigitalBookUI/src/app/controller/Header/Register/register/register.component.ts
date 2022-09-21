import { Component, OnInit } from '@angular/core';
import { User } from 'src/app/models/user';
import { UsersService } from 'src/app/services/users.services';
import { Router } from '@angular/router';
import { catchError, map, of, Subscription } from 'rxjs';
import { EMPTY_OBSERVER } from 'rxjs/internal/Subscriber';
import { NotificationService } from 'src/app/services/notification.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  displayStyle = "block";
  users: User[] = [];
  user: User = {
    userId: 0,
    userName: '',
    firstName: '',
    lastName: '',
    emailId: '',
    userPassword: '',
    roleId: 0,
    active: true
  }

  roles: any[] = [
    { id: 1, name: 'Author' },
    { id: 2, name: 'Reader' }
  ];

  msg:string='';

  constructor(private usersService: UsersService,private router:Router,private notifyService : NotificationService) { }

  ngOnInit(): void {
    this.openPopup();
  }

  openPopup() {
    this.displayStyle = "block";
  }

  closePopup() {
    this.displayStyle = "none";
    //location.href='../';
    this.router.navigateByUrl('/');
  }

  getAllUsers() {
    this.usersService.getAllUsers()
      .subscribe(
        response => { this.users = response }
      );
  }

  onSubmit() {
    if (this.user.userId === 0) {
      this.usersService.saveUser(this.user)
        .subscribe(
          response => {
            if(response.userId>0)
            {
              this.msg="User Added Successfully";
              //alert(this.msg);
              this.notifyService.showSuccess(this.msg,"Digital Book");
            }
            this.getAllUsers();
            this.user = {
              userId: 0,
              userName: '',
              firstName: '',
              lastName: '',
              emailId: '',
              userPassword: '',
              roleId: 0,
              active: true
            };
          }
        );
     }
  }

}
