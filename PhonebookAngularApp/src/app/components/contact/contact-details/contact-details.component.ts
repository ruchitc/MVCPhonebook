import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ApiResponse } from 'src/app/models/ApiResponse{T}';
import { Contact } from 'src/app/models/contact.model';
import { ContactService } from 'src/app/services/contact.service';

@Component({
  selector: 'app-contact-details',
  templateUrl: './contact-details.component.html',
  styleUrls: ['./contact-details.component.css']
})
export class ContactDetailsComponent implements OnInit {
  loading: boolean = false;
  contact: Contact = {
    contactId: 0,
    firstName: '',
    lastName: '',
    contactNumber: '',
    gender: '',
    countryId: 0,
    stateId: 0,
    isFavourite: false,
    email: '',
    company: '',
    fileName: '',
    imageBytes: undefined,
    country: {
      countryId: 0,
      countryName: ''
    },
    state: {
      stateId: 0,
      stateName: '',
      countryId: 0
    },
  }
  formData!: FormData;

  constructor(private contactService: ContactService, private router: Router, private currentRoute: ActivatedRoute) {
    const contactId = this.currentRoute.snapshot.paramMap.get('contactId');
    this.loadContactDetails(Number(contactId));
  }

  ngOnInit(): void {
  }

  loadContactDetails(contactId: number): void {
    this.loading = false;
    this.contactService.getContactById(contactId).subscribe({
      next:(response: ApiResponse<Contact>) => {
        if(response.success) {
          this.contact = response.data;
        } else {
          alert('Failed to get contact: ' + response.message);
        }
        this.loading = false;
      },
      error:(errorResponse) => {
        alert('Error: ' + errorResponse.error.message);
        this.loading = false;
        this.router.navigate(['/contacts']);
      }
    })
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
          this.loading = false;
          this.router.navigate(['/contacts']);
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

  changeFavouriteStatus() {
    this.formData = new FormData;
    this.contact.isFavourite = !this.contact.isFavourite;

    this.formData.append('ContactId', this.contact.contactId.toString());
    this.formData.append('FirstName', this.contact.firstName);
    this.formData.append('LastName', this.contact.lastName);
    this.formData.append('ContactNumber', this.contact.contactNumber);
    this.formData.append('Gender', this.contact.gender);
    this.formData.append('CountryId', this.contact.countryId.toString());
    this.formData.append('StateId', this.contact.stateId.toString());
    this.formData.append('IsFavourite', this.contact.isFavourite.toString());
    
    if(this.contact.email == null) {
      this.formData.append('Email', '');
    }
    else {
      this.formData.append('Email', this.contact.email);
    }

    if(this.contact.company == null) {
      this.formData.append('Company', '');
    }
    else {
      this.formData.append('Company', this.contact.company);
    }

    this.contactService.updateContact(this.formData).subscribe({
      next:(response: ApiResponse<string>) => {
        if(response.success) {
        } else {
          alert('Failed to update contact: ' + response.message);
        }
      },
      error: (errorResponse) => {
        alert('Error while updating contact: ' + errorResponse.error.message);
      }
    })
  }
}
