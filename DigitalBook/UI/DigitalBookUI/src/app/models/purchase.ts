import { Book } from "./book";

export interface purchase {
    purchaseId: number,
    emailId : string,
    bookId : number,
    purchaseDate:Date,
    paymentMode : string,
    isRefund : boolean,
    purchaseStatus:string,
    book:Book|null
}