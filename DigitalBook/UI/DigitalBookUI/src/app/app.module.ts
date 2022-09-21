import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HttpClientModule, HTTP_INTERCEPTORS} from '@angular/common/http'
import { SearchBookComponent } from 'src/app/controller/Book/search-book/search-book.component';
import { FormsModule,ReactiveFormsModule  } from '@angular/forms';
import { HeaderComponent } from './controller/Header/header/header.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import {MatMenuModule} from '@angular/material/menu';
import { FooterComponent } from './controller/Footer/footer/footer.component';
import { LoginComponent } from './controller/Header/Login/login/login.component';
import { RegisterComponent } from './controller/Header/Register/register/register.component';
import { CreateBookComponent } from './controller/Book/createBook/create-book/create-book.component';
import { PurchasebookhistoryComponent } from './controller/Book/PurchaseBookHistory/purchasebookhistory/purchasebookhistory.component';
import { ReaderComponent } from './controller/Book/reader/reader/reader.component';
import { TokenService } from './services/token.service';
import { BooksService } from './services/books.service';
import { UsersService } from './services/users.services';
import { HeaderService } from './services/header.service';

import { ToastrModule } from 'ngx-toastr';

@NgModule({
  declarations: [
    AppComponent,
    SearchBookComponent,
    HeaderComponent,
    FooterComponent,
    LoginComponent,
    RegisterComponent,
    CreateBookComponent,
    PurchasebookhistoryComponent,
    ReaderComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    BrowserAnimationsModule,
    MatMenuModule,
    ToastrModule.forRoot()
  ],
  providers: [
    {provide: HTTP_INTERCEPTORS,useClass:TokenService,multi:true},
    BooksService,
    UsersService,
    HeaderService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
