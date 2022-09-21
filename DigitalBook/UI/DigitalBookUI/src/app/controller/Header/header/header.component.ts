import { Component, OnInit} from '@angular/core';
import { Router } from '@angular/router';
import { HeaderService } from 'src/app/services/header.service';
import { UsersService } from 'src/app/services/users.services';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {
  constructor(public router:Router,private hederService:HeaderService) { }

  ModalTitle:string="";
  ActivateSignupComp :boolean=false;

  userLoggedIn :boolean =false;
  showSignInSignUp : boolean = true;

  display = "none";
  SignupModaldisplay ="none";

  name:string='';

  openModal() {
    this.ModalTitle ="Sign Up";
    this.SignupModaldisplay = "block";
    this.display = "none";
  }

  openSignInModal() {
    this.ModalTitle ="Sign In";
    this.display = "block";
    this.SignupModaldisplay = "none";
  }
  onCloseHandled() {
    this.display = "none";
    this.SignupModaldisplay = "none";
  }

  ngOnInit(): void {
    this.userLoggedIn = this.hederService.CheckUserLoggedInOrNot();
    this.isUserLoggedIn(this.userLoggedIn);
    if(localStorage.getItem("User")!=null){
      let values = JSON.parse(localStorage.getItem("User") || '');
      this.name = values.userName;
    }
  }

  signOutClick() {
    // remove user from local storage to log user out
    localStorage.removeItem('Token');
    localStorage.removeItem('User');
    this.isUserLoggedIn(false);
    this.name ='';
    this.router.navigate(['/']);        
  } 

  isUserLoggedIn(loggedIn:boolean){
    if(loggedIn){
      this.showSignInSignUp =false;
    }
    else{
      this.showSignInSignUp =true;
    }
  }

  isUserLogin(){
    this.userLoggedIn = this.hederService.CheckUserLoggedInOrNot();
    if(localStorage.getItem("User")!=null){
      return true;
    }
    else{
      return false;
    }
  }

 
}
