import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { SecurityQuestion } from 'src/app/models/security-question.model';
import { User } from 'src/app/models/user.model';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.css']
})
export class SignupComponent {
  loading: boolean = false;
  user: User = {
    userId: 0,
    firstName: '',
    lastName: '',
    loginId: '',
    email: '',
    contactNumber: '',
    password: '',
    confirmPassword: '',
    securityQuestionId_1: 0,
    securityAnswer_1: '',
    securityQuestionId_2: 0,
    securityAnswer_2: ''
  }

  securityQuestions_1: SecurityQuestion[] = [];
  securityQuestions_2: SecurityQuestion[] = [];

  constructor(private authService: AuthService, private router: Router) {

  }

  ngOnInit() {
    this.getSecurityQuestions();
  }

  getSecurityQuestions(): void {
    this.authService.getSecurityQuestions().subscribe({
      next:(response) => {
        if(response.success) {
          this.securityQuestions_1 = response.data;
          this.securityQuestions_2 = response.data;
        } else {
          alert(response.message);
        }
        this.loading = false;
      },
      error:(err) => {
        alert(err.error.message);
        this.loading = false;
      }
    })
    }

  checkPasswords(form: NgForm):void {
    const password = form.controls['password'];
    const confirmPassword = form.controls['confirmPassword'];
 
    if (password && confirmPassword && password.value !== confirmPassword.value) {
      confirmPassword.setErrors({ passwordMismatch: true });
    } else {
      confirmPassword.setErrors(null);
    }
  }

  onSubmit(signUpForm: NgForm): void {
    if (signUpForm.valid) {
      this.loading = true;
      console.log(signUpForm.value);
      this.authService.signUp(this.user).subscribe({
        next:(response) => {
          if(response.success) {
            this.router.navigate(['/signupsuccess'])
          } else {
            alert(response.message);
          }
          this.loading = false;
        },
        error:(err) => {
          alert(err.error.message);
          this.loading = false;
        }
      })
    }
  }
}
