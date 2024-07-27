import { ChangeDetectorRef, Component } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { UpdatePassword } from 'src/app/models/update-password.model';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-update-password',
  templateUrl: './update-password.component.html',
  styleUrls: ['./update-password.component.css']
})
export class UpdatePasswordComponent {
  username: string | null | undefined;
  updatePassword: UpdatePassword = {
    loginId: '',
    oldPassword: '',
    newPassword: '',
    confirmNewPassword: ''
  }
  updatePasswordSuccess: boolean = false;

  constructor(private authService: AuthService, private cdr: ChangeDetectorRef, private router: Router) {
    
  }

  ngOnInit(): void {
    this.authService.getUsername().subscribe((username: string | null | undefined) =>  {
      this.username = username;
      this.cdr.detectChanges();
    })

    this.updatePassword.loginId = this.username;
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

  onSubmit(updatePasswordForm: NgForm): void {
    if (updatePasswordForm.valid) {
      console.log(updatePasswordForm.value);
      this.authService.updatePassword(this.updatePassword).subscribe({
        next:(response) => {
          if(response.success) {
            this.updatePasswordSuccess = true;
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
