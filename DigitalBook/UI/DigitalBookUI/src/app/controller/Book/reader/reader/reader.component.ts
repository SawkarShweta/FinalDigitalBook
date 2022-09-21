import { Component, OnInit, ViewChild } from '@angular/core';
import { HeaderComponent } from 'src/app/controller/Header/header/header.component';
import { Book } from 'src/app/models/book';
import { purchase } from 'src/app/models/purchase';
import { User } from 'src/app/models/user';
import { HeaderService } from 'src/app/services/header.service';

@Component({
  selector: 'app-reader',
  templateUrl: './reader.component.html',
  styleUrls: ['./reader.component.css']
})
export class ReaderComponent implements OnInit {
  userEmailID : string ="";
  user:User={
    userId:0,
    userName:'',
    firstName:'',
    lastName:'',
    emailId:'',
    userPassword:'',
    roleId:0,
    active:true
  }

  

  books:Book[] = [];
    book :Book= {
      bookId:0,
      categoryId :0,
      userId: 0,
      bookName:'',
      price:0,
      publisher:'',
      publishedDate:new Date(),
      content:'',
      active:true,
      createdDate:new Date(),
      createdby:0,
      modifiedDate:new Date(),
      modifiedby:0,
      user:this.user,
    }
 
    display : string = 'none';
    ModalTitle="Purchase Book";
    readBookdisplay : string ="none";
    ModalReadBookTitle : string ="Read Book";
    bookID : any;

    puchases:purchase[] = [];
    objpurchase : purchase={
      purchaseId: 0,
      emailId : '',
      bookId : 0,
      purchaseDate:new Date,
      paymentMode : '',
      isRefund : false,
      purchaseStatus:'Completed',
      book:this.book
    }

    constructor(private services:HeaderService){}

    ngOnInit(): void {
      this.GetUserID();
      this.loadBookHistory();
    }
    purchaseClick(item:purchase){
        this.objpurchase.book =item.book; 
        this.bookID= this.book.bookId;
        if(item.book != null)
          this.book=item.book;
        //this.book.bookId= this.book.bookId;
        this.display= 'block';
    }
    onCloseHandled() {
        this.display = "none";
        this.readBookdisplay ="none";

      }

      onCloseHandledofpurchase() {
        this.display = "none";
        this.readBookdisplay ="none";
        window.location.reload();
      }

      GetUserID(){
        let values = JSON.parse(localStorage.getItem("User") || '');
        this.userEmailID = values.emailId;
        this.book.user =values;
      }

      loadBookHistory(){
    
        this.services.GetBookListReader(this.userEmailID).subscribe(
          response => {this.puchases = response; }
        )
      }

      readBookClick(item:purchase){
        this.objpurchase.book =item.book; 
        if(this.objpurchase.book)
          this.objpurchase.book.content= this.objpurchase.book.content;
        this.readBookdisplay= 'block';
      }
}
