import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, Observable,of,throwError } from 'rxjs';
import { User } from '../models/user';
import { NotificationService } from './notification.service';

@Injectable({
  providedIn: 'root'
})
export class UsersService {

  baseUrl = "https://localhost:7044/api/UserMasters";
  authUrl= "https://localhost:7214/validateUser";
  constructor(private http: HttpClient,private notifyService : NotificationService) { }
  errorMsg ="";

  //Get all Users
  getAllUsers():Observable<User[]>{
      return this.http.get<User[]>(this.baseUrl).pipe(
        catchError((error:any)=>{return this.errorHandler(error)})
      );
  }

  getUserByUserName(userName:string):Observable<User>{
    return this.http.get<User>(this.baseUrl+'/GetUserMasterByName?name='+userName).pipe(
      catchError((error:any)=>{return this.errorHandler(error)})
    );
  }

  saveUser(user: User):Observable<User> {
    return this.http.post<User>(this.baseUrl, user).pipe(
      catchError((error:any)=>{return this.errorHandler(error)})
    );
  }

  loginUser(user: User):Observable<any>{
    var request={
      userName:user.userName,
      password:user.userPassword
    }
    return this.http.post<any>(this.authUrl,request).pipe(
      catchError((error:any)=>{return this.errorHandler(error)})
    );
  }

  errorHandler(error: { error: { message: string;detail:string }; status: any; message: any; }) {
    let errorMessage = '';
    if(error.error instanceof ErrorEvent) {
      errorMessage = error.error.message;
    } else {
      errorMessage = error.error.detail;
    }
    //alert(errorMessage);
    this.notifyService.showError(errorMessage,"DigitalBook");
    return throwError(errorMessage);
  }
}
