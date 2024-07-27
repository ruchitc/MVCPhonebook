import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ApiResponse } from 'src/app/models/ApiResponse{T}';
import { PaginationApiResponse } from 'src/app/models/PaginationApiResponse{T}';
import { Contact } from 'src/app/models/contact.model';
import { AuthService } from 'src/app/services/auth.service';
import { ContactService } from 'src/app/services/contact.service';

@Component({
  selector: 'app-contact-list',
  templateUrl: './contact-list.component.html',
  styleUrls: ['./contact-list.component.css']
})
export class ContactListComponent implements OnInit {
  contacts!: Contact[] | null;
  isAuthenticated: boolean = false;
  loading: boolean = false;
  searchArray: string[] = ['A', 'B', 'C', 'D', 'E','F','G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'];
  
  page: number = 1;
  pageSize: number = 6;
  totalPages: number = 0;
  searchString: string = '';
  sortDir: string = 'default';
  showFavourites: boolean = false;

  formData!: FormData;

  constructor(private contactService: ContactService, private authService: AuthService, private cdr: ChangeDetectorRef, private router: Router) {
    
  }

  ngOnInit(): void {
    this.formData = new FormData;
    this.loadContacts(this.page, this.searchString, this.sortDir, this.showFavourites);
    this.authService.isAuthenticated().subscribe((authState: boolean) => {
      this.isAuthenticated = authState;
      this.cdr.detectChanges(); // Manually trigger change detection.
    });
  }

  loadContacts(page: number, searchString: string, sortDir: string, showFavourites: boolean): void {
    this.page = page
    this.searchString = searchString;
    this.sortDir = sortDir;
    this.showFavourites = showFavourites;

    this.loading = true;
    this.contactService.getAllContacts(this.page, this.pageSize, this.searchString, this.sortDir, this.showFavourites).subscribe({
        next:(response: PaginationApiResponse<Contact[]>) => {
          if(response.success) {
            this.totalPages = Math.ceil(response.total / this.pageSize);
            this.contacts = response.data;
          } else {
            console.error('Failed to fetch contacts', response.message)
          }
          this.loading = false;
        },
        error: (error) => {
          this.totalPages = 0;
          this.contacts = null;
          this.loading = false;
        }
    })
  }

  sortColumn() {
    if(this.sortDir == 'default') {
      this.sortDir = 'asc'
    }
    else if(this.sortDir == 'asc') {
      this.sortDir = 'desc'
    }
    else if(this.sortDir == 'desc') {
      this.sortDir = 'default'
    }

    this.loadContacts(1, this.searchString, this.sortDir, this.showFavourites);
  }

  changeFavouriteStatus(contactId: number) {
    let contact: Contact | undefined = this.contacts?.find(c => c.contactId == contactId)
    if(contact !== undefined) {
      this.formData = new FormData;
      contact.isFavourite = !contact.isFavourite;
  
      this.formData.append('ContactId', contact.contactId.toString());
      this.formData.append('FirstName', contact.firstName);
      this.formData.append('LastName', contact.lastName);
      this.formData.append('ContactNumber', contact.contactNumber);
      this.formData.append('Gender', contact.gender);
      this.formData.append('CountryId', contact.countryId.toString());
      this.formData.append('StateId', contact.stateId.toString());
      this.formData.append('IsFavourite', contact.isFavourite.toString());
      
      if(contact.email == null) {
        this.formData.append('Email', '');
      }
      else {
        this.formData.append('Email', contact.email);
      }
  
      if(contact.company == null) {
        this.formData.append('Company', '');
      }
      else {
        this.formData.append('Company', contact.company);
      }
  
      this.contactService.updateContact(this.formData).subscribe({
        next:(response: ApiResponse<string>) => {
          if(response.success) {
            if(this.showFavourites) {
              this.loadContacts(this.page, this.searchString, this.sortDir, this.showFavourites);
            }
          } else {
            alert('Failed to update contact: ' + response.message);
          }
        },
        error: (errorResponse) => {
          alert('Error while updating contact: ' + errorResponse.error.message);
        }
      })
    }
    else {
      console.log("Contact not found");
    }
  }

  confirmDelete(contactId : number): void{
    if(confirm('Are you sure?')){
      this.deleteContact(contactId);
    }
  }
 
  deleteContact(contactId: number): void{
    this.loading = true;
    this.contactService.deleteContact(contactId).subscribe({
      next:(response) => {
        if(response.success){
          this.loadContacts(1, this.searchString, this.sortDir, false);
          this.loading = false;
        }
        else{
          alert(response.message);
          this.loading = false;
        }
      },
      error: (err) =>{
        alert(err.error.message);
        this.loading = false;
      }
    });
  }
}
