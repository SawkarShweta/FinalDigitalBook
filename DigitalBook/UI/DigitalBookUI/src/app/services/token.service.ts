import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class TokenService implements HttpInterceptor {

  constructor() { }
  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    ///

    let token= localStorage.getItem('Token');

    let jwtToken = req.clone({
      setHeaders:{
        Authorization : 'bearer ' +token
      }
    })
    return next.handle(jwtToken);
  }
}
