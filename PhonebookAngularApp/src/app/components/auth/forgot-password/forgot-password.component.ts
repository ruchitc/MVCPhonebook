import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { UserSecurityQuestions } from 'src/app/models/user-security-questions.model';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.css']
})
export class ForgotPasswordComponent {
  username: string | null | undefined;
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

  constructor(private authService: AuthService, private router: Router) {
    
  }

  onSubmit(ForgotPasswordForm: NgForm): void {
    if (ForgotPasswordForm.valid) {
      this.authService.getUserSecurityQuestions(this.username).subscribe({
        next:(response) => {
          if(response.success) {
            this.userSecurityQuestions = response.data;
            this.router.navigate(['/auth/reset-password/'], { state: { data: this.userSecurityQuestions } });
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
