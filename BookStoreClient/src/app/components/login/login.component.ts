import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { Money } from 'src/app/models/book.model';
import { SetShoppingCartsModel } from 'src/app/models/set-shopping-carts.model';
import { AuthService } from 'src/app/services/auth.service';
import { ErrorService } from 'src/app/services/error.service';
import { ShoppingCartService } from 'src/app/services/shopping-cart.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {

  constructor(
    private http: HttpClient,
    private router:Router,
    private auth: AuthService,
    private shoppingCart: ShoppingCartService,
    private error : ErrorService
  ){

  }

  signIn(form: NgForm){
    if(form.valid){
      this.http.post(
        "https://localhost:7250/api/Auth/Login",
        {
          usernameOrEmail: form.controls["userNameOrEmail"].value, 
          password: form.controls["password"].value
        })
      .subscribe({
        next: (res:any)=> {
          localStorage.setItem("response",JSON.stringify(res));
          this.auth.isAuthentication();
          
          const request:SetShoppingCartsModel[] = [];
  
          if(this.shoppingCart.shoppingCarts.length > 0){
            for(let s of this.shoppingCart.shoppingCarts){
              const cart = new SetShoppingCartsModel();
              cart.bookId = s.id;
              cart.userId = this.auth.userId;
              cart.price = s.price;
              cart.quantity = 1;
              
              request.push(cart);
            }
    
            this.http.post("https://localhost:7250/api/ShoppingCarts/SetShoppingCartsFromLocalStorage", request).subscribe(res=> {
            localStorage.removeItem("shoppingCarts");
            this.shoppingCart.getAllShoppingCarts();
            });
          }
  
          
         
          this.router.navigateByUrl("/");
        },

        error: (err: HttpErrorResponse)=> {
          this.error.errorHandler(err);
        }
      })
    }
  }
}