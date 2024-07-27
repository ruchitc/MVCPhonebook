import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { UpdateUserDetails } from 'src/app/models/update-user-details.model';
import { UserDetails } from 'src/app/models/user-details.model';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-update-user-details',
  templateUrl: './update-user-details.component.html',
  styleUrls: ['./update-user-details.component.css']
})
export class UpdateUserDetailsComponent {
  user: UserDetails = {
    userId: 0,
    firstName: '',
    lastName: '',
    loginId: '',
    contactNumber: '',
    email: ''
  }

  constructor(private authService: AuthService, private router: Router) {

  }

  ngOnInit() {
    this.user = history.state.data;
  }

  onSubmit(updateUserDetailsForm: NgForm): void {
    if (updateUserDetailsForm.valid) {
      this.authService.updateUserDetails(this.user).subscribe({
        next:(response) => {
          if(response.success) {
            this.router.navigate(['/user/profile']);
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
