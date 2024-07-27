import { ChangeDetectorRef, Component } from '@angular/core';
import { ApiResponse } from 'src/app/models/ApiResponse{T}';
import { UserDetails } from 'src/app/models/user-details.model';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.css']
})
export class UserProfileComponent {
  username: string | null | undefined;
  user: UserDetails = {
    userId: 0,
    firstName: '',
    lastName: '',
    loginId: '',
    email: '',
    contactNumber: ''
  }

  constructor(private authService: AuthService, private cdr: ChangeDetectorRef) {
    
  }

  ngOnInit(): void {
    this.authService.getUsername().subscribe((username: string | null | undefined) =>  {
      this.username = username;
      this.cdr.detectChanges();
    })

    this.loadUserDetails();
  }

  loadUserDetails(): void {
    this.authService.getUserDetails(this.username).subscribe({
      next:(response: ApiResponse<UserDetails>) => {
        if(response.success) {
          this.user = response.data;
        } else {
          alert('Failed to get user: ' + response.message);
        }
      },
      error:(errorResponse) => {
        alert('Error: ' + errorResponse.error.message);
      }
    })
  }
}
