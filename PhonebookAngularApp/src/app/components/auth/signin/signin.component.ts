import { ChangeDetectorRef, Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';
import { LocalStorageService } from 'src/app/services/helpers/local-storage.service';
import { LocalStorageKeys } from 'src/app/services/helpers/localstoragekeys';

@Component({
  selector: 'app-signin',
  templateUrl: './signin.component.html',
  styleUrls: ['./signin.component.css']
})
export class SigninComponent {
  loading: boolean = false;
  username: string = '';
  password: string = '';

  constructor(
    private authService: AuthService, 
    private localStorageHelper: LocalStorageService, 
    private router: Router, 
    private cdr: ChangeDetectorRef) {

  }

  login() {
    this.loading = true;
    this.authService.signIn(this.username, this.password).subscribe({
      next:(response) => {
        if(response.success) {
          this.localStorageHelper.setItem(LocalStorageKeys.TokenName, response.data);
          this.localStorageHelper.setItem(LocalStorageKeys.UserId, this.username);
          this.cdr.detectChanges();
          this.router.navigate(['/home']);
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
