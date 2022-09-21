import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Book } from 'src/app/models/book';
import { purchase } from 'src/app/models/purchase';
import { User } from 'src/app/models/user';
import { HeaderService } from 'src/app/services/header.service';
import { NotificationService } from 'src/app/services/notification.service';

@Component({
  selector: 'app-purchasebookhistory',
  templateUrl: './purchasebookhistory.component.html',
  styleUrls: ['./purchasebookhistory.component.css']
})
export class PurchasebookhistoryComponent implements OnInit {
  display = "none";

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

  bookHistoryList:purchase[] = [];
  @Input() book : Book = {
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

  constructor(private services: HeaderService,private router:Router, private notifyService: NotificationService) { }

  ngOnInit(): void {
  }

  loadBookHistory(){
    
    this.services.GetBookHistory(this.objpurchase.emailId).subscribe(
      response => {this.bookHistoryList = response;
        console.log(this.bookHistoryList);
        this.display = "block";
      }
    )
  }

  onSubmit(){
    this.objpurchase.bookId = this.book.bookId;
    this.objpurchase.book=null;
    this.services.PurchaseBook(this.objpurchase).subscribe(
      response => {
      //alert("Book Purchased Successfully.");
        this.notifyService.showSuccess("Book Purchased Successfully.","DigitalBook");
      this.loadBookHistory(); }
    )
  }
  onFocusOutEvent(event: any){
    this.loadBookHistory();
  }
}
