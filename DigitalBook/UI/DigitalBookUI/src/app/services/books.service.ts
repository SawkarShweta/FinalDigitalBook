import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, Observable, throwError } from 'rxjs';
import { Book } from '../models/book';

@Injectable({
  providedIn: 'root'
})
export class BooksService {

  baseUrl = "https://localhost:7044/api/Books";

  constructor(private http: HttpClient) { }

  //Get all Books
  getAllBooks():Observable<Book[]>{
      return this.http.get<Book[]>(this.baseUrl).pipe(
        catchError((error:any)=>{return this.errorHandler(error)})
      );
  }

  getAllBooksById(id:number):Observable<Book[]>{
    return this.http.get<Book[]>(this.baseUrl+'/GetAllBooksByUserId?id='+id).pipe(
      catchError((error:any)=>{return this.errorHandler(error)})
    );
  }

  //https://localhost:7044/api/Books/searchBook?title=abc&author=rr&publisher=string&releasedDate=2022-09-07
  searchBook(title:string,author:string,publisher:string,releasedDate:Date):Observable<Book[]>{
    return this.http.get<Book[]>(this.baseUrl+'/searchBook?title='+title+'&author='+author+'&publisher='+publisher+'&releasedDate='+releasedDate).pipe(
      catchError((error:any)=>{return this.errorHandler(error)})
    );
  }

  getAllCategories():Observable<any[]>{
    return this.http.get<any[]>(this.baseUrl+'/GetAllCategories').pipe(
      catchError((error:any)=>{return this.errorHandler(error)})
    );
  }

  saveBook(book: Book):Observable<Book> {
    return this.http.post<Book>(this.baseUrl, book).pipe(
      catchError((error:any)=>{return this.errorHandler(error)})
    );
  }

  blockUnblockBook(id:number):Observable<Book>{
    return this.http.put<Book>(this.baseUrl+'/blockUnblockBook?id='+id,id).pipe(
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
    alert(errorMessage);
    return throwError(errorMessage);
  }
}
