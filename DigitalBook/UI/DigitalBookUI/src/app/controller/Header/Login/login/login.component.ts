import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { throwError } from 'rxjs';
import { User } from 'src/app/models/user';
import { HeaderService } from 'src/app/services/header.service';
import { UsersService } from 'src/app/services/users.services';
import { HeaderComponent } from '../../header/header.component';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  displayStyle = "block";
  loginfailed=false;
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

 token:string='';
 isAuthenticated:boolean=false;

  constructor(private router:Router,
    private usersService:UsersService,
    private headerService:HeaderService
    ) { }
  ngOnInit(){
    this.loginfailed=false;
  }
  
  openPopup() {
    this.displayStyle = "block";
  }
  closePopup() {
    this.displayStyle = "none";
    //location.href='../';
    this.router.navigateByUrl('/');
  }

  // onSubmit() {
  //   this.usersService.loginUser(this.user)
  //     .subscribe(
  //       (response: { token: string; isAuthenticated: boolean; }) => { 
  //         if(response.isAuthenticated ==true){
  //           localStorage.setItem("Token",response.token);
  //           this.usersService.getUserByUserName(this.user.userName).subscribe(
  //             response => { 
  //               this.user = response;
  //               localStorage.setItem("User",JSON.stringify(this.user));
  //                 let headerComponentObj = new HeaderComponent(this.router,this.headerService);
  //                 headerComponentObj.ngOnInit();

  //                 // this.nameEmitter.emit(true);  
  //                 if(this.user.roleId == 1) //This is Author
  //                 {
  //                   this.router.navigateByUrl('/createBook').then(()=>{window.location.reload()});
  //                   //this.router.navigateByUrl('/createBook');
  //                 }
  //                 else{ 
  //                     // This is Reader
  //                     this.router.navigateByUrl('/reader').then(()=>{window.location.reload()});
  //                     //this.router.navigateByUrl('/reader');
  //                 }
  //             }
  //           )
  //         }
  //         else
  //           localStorage.removeItem("Token");
  //           //alert("Incorrect UserName or Password");
  //           this.loginfailed=true;
  //       }
  //     );
  // }

  login() {
    this.usersService.loginUser(this.user)
      .subscribe(
        (response: { token: string; isAuthenticated: boolean;user:User; }) => { 
          if(response.isAuthenticated ==true){
            localStorage.setItem("Token",response.token);
            this.user = response.user;
            localStorage.setItem("User",JSON.stringify(this.user));
            let headerComponentObj = new HeaderComponent(this.router,this.headerService);
            headerComponentObj.ngOnInit();
            if(this.user.roleId == 1) //This is Author
            {
               this.router.navigateByUrl('/createBook').then(()=>{window.location.reload()});
            }
             else{ 
                this.router.navigateByUrl('/reader').then(()=>{window.location.reload()});
            }
          }
          else
            localStorage.removeItem("Token");
            this.loginfailed=true;
        }
      );
  }
}
