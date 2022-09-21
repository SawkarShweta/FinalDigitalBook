import { User } from "./user";

export interface Book {
    bookId:number;
    categoryId :number;
    userId: number;
    bookName:string;
    price:number;
    publisher:string;
    publishedDate:Date;
    content:string;
    active:boolean;
    createdDate:Date;
    createdby:number;
    modifiedDate:Date;
    modifiedby:number;
    user:User|null;
}