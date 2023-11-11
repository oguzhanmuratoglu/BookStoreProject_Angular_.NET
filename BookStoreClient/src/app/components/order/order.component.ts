import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Component } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { BookModel, Money } from 'src/app/models/book.model';
import { OrderStatusEnum } from 'src/app/models/order-status-enum';
import { OrderModel } from 'src/app/models/order.model';
import { AuthService } from 'src/app/services/auth.service';
import { ErrorService } from 'src/app/services/error.service';

@Component({
  selector: 'app-order',
  templateUrl: './order.component.html',
  styleUrls: ['./order.component.css']
})
export class OrderComponent {
  orders: OrderModel[] = [];
  language: string = "en";
  orderStatusEnum = OrderStatusEnum;
  selectedOrder: OrderModel = new OrderModel();

  constructor(
    private http: HttpClient,
    private auth: AuthService,
    private err: ErrorService,
    private translate: TranslateService
  ){
    if (localStorage.getItem("language")) {
      this.language = localStorage.getItem("language") as string;
    }
    this.getAll();
  }

  getAll(){
    this.auth.isAuthentication();
    this.http.get("https://localhost:7250/api/Orders/GetAllByUserId/" + this.auth.userId).subscribe({
      next: (res: any)=> {
        this.orders = res;
      },
      error: (err: HttpErrorResponse)=> {
        this.err.errorHandler(err);
      }
    })
  }

  translateOrderStatus(status: string){
    return this.translate.get(status);
  }

  hasTheReturnPeriodPassed(statusDate: string){
    const returnPeriod = 14; // 14 gün
    const statusDateObj = new Date(statusDate);
    const currentDate = new Date();

    // statusDate'den itibaren geçen zamanın milisaniye cinsinden hesaplanması
    const timeDiff = currentDate.getTime() - statusDateObj.getTime();

    // Zaman farkının gün cinsinden hesaplanması
    const diffDays = timeDiff / (1000 * 3600 * 24);

    // 14 günden az ise true, değilse false dön
    return diffDays < returnPeriod;
  }

  selectedOrderForComment(order: OrderModel){
    this.selectedOrder = {...order};
  }

  saveComment(){
    this.http.post("https://localhost:7250/api/Orders/SaveComment", {orderId: this.selectedOrder.id, comment: this.selectedOrder.comment, raiting: this.selectedOrder.raiting}).subscribe({
      next: (res:any)=> {
        const el = document.getElementById("commentModalCloseBtn")
        el?.click();
        this.getAll();
      },
      error: (err: HttpErrorResponse)=> {
        this.err.errorHandler(err);
      }
    })
  }
}