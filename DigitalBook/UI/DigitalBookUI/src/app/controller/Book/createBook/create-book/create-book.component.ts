import { Component, OnInit } from '@angular/core';
import { BooksService } from 'src/app/services/books.service';
import { Book } from 'src/app/models/book';
import { User } from 'src/app/models/user';
import { HeaderService } from 'src/app/services/header.service';
import { NotificationService } from 'src/app/services/notification.service';

@Component({
  selector: 'app-create-book',
  templateUrl: './create-book.component.html',
  styleUrls: ['./create-book.component.css']
})
export class CreateBookComponent implements OnInit {
  msg: string = '';

  constructor(private booksService: BooksService, private headerService: HeaderService, private notifyService: NotificationService) { }
  user: User = JSON.parse(localStorage.getItem('User') || '{}');

  books: Book[] = [];
  book: Book = {
    bookId: 0,
    categoryId: 0,
    userId: this.user.userId,
    bookName: '',
    price: 0,
    publisher: '',
    publishedDate: new Date(),
    content: '',
    active: true,
    createdDate: new Date(),
    createdby: 0,
    modifiedDate: new Date(),
    modifiedby: 0,
    user: null
  }

  LoggedUserId: number | 0 = 0;

  categories: any = this.getAllCategories();
  ngOnInit(): void {
    //this.getAllCategories();
    this.headerService.CheckUserLoggedInOrNot();
    this.GetUserID();
    this.getAllBooksById();
  }

  GetUserID() {
    let values = JSON.parse(localStorage.getItem("User") || '');
    this.LoggedUserId = values.userId;
  }

  getAllCategories() {
    this.booksService.getAllCategories()
      .subscribe(
        response => { this.categories = response; }
      );
  }

  getAllBooks() {
    this.booksService.getAllBooks()
      .subscribe(
        response => { this.books = response }
      );
  }

  getAllBooksById() {
    this.booksService.getAllBooksById(this.LoggedUserId)
      .subscribe(
        response => { this.books = response }
      );
  }

  onSubmit() {
    if (this.book.bookId === 0) {
      this.booksService.saveBook(this.book)
        .subscribe(
          response => {
            if (response.bookId > 0) {
              this.msg = "Book Saved Successfully";
              //alert(this.msg);
              this.notifyService.showSuccess(this.msg, "Digital Book");
            }
            this.getAllBooksById();
            this.book = {
              bookId: 0,
              categoryId: 0,
              userId: 0,
              bookName: '',
              price: 0,
              publisher: '',
              publishedDate: new Date(),
              content: '',
              active: true,
              createdDate: new Date(),
              createdby: 0,
              modifiedDate: new Date(),
              modifiedby: 0,
              user: this.user
            };
          }
        );
    }
    else {
      //this.updateCard(this.card);
    }
  }

  blockBook(book: Book, btn: any) {
    this.booksService.blockUnblockBook(book.bookId)
      .subscribe(
        response => {
          btn.value = response.active == true ? "Block" : "Unblock";
          if(response.active == true)
            this.notifyService.showSuccess("Book Unblocked","Digital Book");
          else
            this.notifyService.showSuccess("Book Blocked","Digital Book");
        }
      );
  }
}
