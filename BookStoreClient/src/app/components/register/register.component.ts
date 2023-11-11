import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { SwalService } from 'src/app/services/swal.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {
constructor(
  private http: HttpClient,
  private swal: SwalService,
  private router: Router
){}

signUp(form: NgForm){
  if(form.valid){
    this.http.post("https://localhost:7250/api/Auth/Register", {
      name: form.controls["name"].value,
      lastname: form.controls["lastName"].value,
      username: form.controls["userName"].value,
      email: form.controls["email"].value,
      password: form.controls["password"].value,
    }).subscribe((res:any)=> {
      this.swal.callToast(res.message,"success");
      this.router.navigateByUrl("/login");
    })
  }
}
}