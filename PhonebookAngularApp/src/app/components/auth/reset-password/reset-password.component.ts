import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ResetPassword } from 'src/app/models/reset-password.model';
import { UserSecurityQuestions } from 'src/app/models/user-security-questions.model';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.css']
})
export class ResetPasswordComponent {
  resetPasswordSuccess: boolean = false;
  resetPassword: ResetPassword = {
    loginId: '',
    securityQuestionId_1: 0,
    securityAnswer_1: '',
    securityQuestionId_2: 0,
    securityAnswer_2: '',
    newPassword: '',
    confirmNewPassword: ''
  }

  userSecurityQuestions: UserSecurityQuestions = {
    loginId: '',
    securityQuestion_1: {
      questionId: 0,
      question: ''
    },
    securityQuestion_2: {
      questionId: 0,
      question: ''
    }
  }

  constructor(private authService: AuthService, private router: Router, private currentRoute: ActivatedRoute) {
  }

  ngOnInit(): void {
    this.userSecurityQuestions = history.state.data;
    this.resetPassword.loginId = this.userSecurityQuestions.loginId;
    this.resetPassword.securityQuestionId_1 = this.userSecurityQuestions.securityQuestion_1.questionId;
    this.resetPassword.securityQuestionId_2 = this.userSecurityQuestions.securityQuestion_2.questionId;
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

  onSubmit(ForgotPasswordForm: NgForm): void {
    if (ForgotPasswordForm.valid) {
      console.log(ForgotPasswordForm.value);
      this.authService.resetPassword(this.resetPassword).subscribe({
        next:(response) => {
          if(response.success) {
            this.resetPasswordSuccess = true;
            setTimeout(() => {
              this.authService.signOut();
            this.router.navigate(['/signin'])
            }, 5000);
          } else {
            alert(response.message);
          }
        },
        error:(err) => {
          alert(err.error.message);
        }
      })
    }
  }
}
