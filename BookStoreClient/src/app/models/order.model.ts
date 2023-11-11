import { BookModel, Money } from "./book.model";
import { OrderStatusModel } from "./order-status.model";

export class OrderModel{
    id:number =0;
    orderNumber ="";
    book :BookModel= new BookModel();
    qunatity:number =0;
    price:Money = new Money();
    createdAt:string ="";
    paymentDate:string ="";
    paymentType:string ="";
    paymentNumber:string ="";
    orderStatuses:OrderStatusModel[] = [];
    comment:string ="";
    raiting:number =0;
}