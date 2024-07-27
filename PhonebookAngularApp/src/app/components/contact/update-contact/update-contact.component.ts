import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ApiResponse } from 'src/app/models/ApiResponse{T}';
import { Contact } from 'src/app/models/contact.model';
import { Country } from 'src/app/models/country.model';
import { State } from 'src/app/models/state.model';
import { UpdateContact } from 'src/app/models/update-contact.model';
import { ContactService } from 'src/app/services/contact.service';
import { CountryService } from 'src/app/services/country.service';
import { StateService } from 'src/app/services/state.service';

@Component({
  selector: 'app-update-contact',
  templateUrl: './update-contact.component.html',
  styleUrls: ['./update-contact.component.css']
})
export class UpdateContactComponent {
  loading: boolean = false;
  contact: UpdateContact = {
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
    image: null
  }
  initialStateId: number = 0;

  formData!: FormData;

  countries: Country[] = [];
  states: State[] = [];

  constructor(
    private contactService: ContactService,
    private countryService: CountryService,
    private stateService: StateService,
    private router: Router,
    private currentRoute: ActivatedRoute) {

  }

  ngOnInit(): void {
    this.formData = new FormData;
    const contactId = this.currentRoute.snapshot.paramMap.get('contactId');
    this.loadContactDetails(Number(contactId));
    this.loadCountries();
  }

  loadCountries(): void {
    this.countryService.getAllCountries().subscribe({
      next:(response: ApiResponse<Country[]>) => {
        if(response.success) {
          this.countries = response.data
        } else {
          console.error('Failed to fetch countries', response.message)
        }
      },
      error: (errorResponse) => {
        console.error('Error fetching countries: ',  errorResponse.error.message);
      }
    })
  }

  loadStatesByCountryId(countryId: number): void {
    this.stateService.getStatesByCountryId(countryId).subscribe({
      next:(response: ApiResponse<State[]>) => {
        if(response.success) {
          this.states = response.data
          
          if(this.initialStateId == -1) {
            this.contact.stateId = this.states[0].stateId;
          }
          this.initialStateId = -1;

        } else {
          console.error('Failed to fetch states', response.message)
        }
      },
      error: (errorResponse) => {
        console.error('Error fetching states: ',  errorResponse.error.message);
      }
    })
  }
  
  loadContactDetails(contactId: number): void {
    this.loading = false;
    this.contactService.getContactById(contactId).subscribe({
      next:(response: ApiResponse<Contact>) => {
        if(response.success) {
          this.contact.contactId = response.data.contactId;
          this.contact.firstName = response.data.firstName;
          this.contact.lastName = response.data.lastName;
          this.contact.contactNumber = response.data.contactNumber;
          this.contact.gender = response.data.gender;
          this.contact.countryId = response.data.countryId;
          this.contact.stateId = response.data.stateId;
          this.contact.isFavourite = response.data.isFavourite;
          this.contact.email = response.data.email;
          this.contact.company = response.data.company;
          this.contact.image = response.data.fileName;

          // Load states only after contact details are loaded
          this.initialStateId = response.data.stateId;
          this.loadStatesByCountryId(this.contact.countryId);
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

  handleFileInput(event: any) {
    this.contact.image = event.target.files[0];
  }

  onSubmit(updateContactForm: NgForm) {
    this.formData = new FormData;

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

    this.formData.append('Image', this.contact.image);

    if(updateContactForm.valid) {
      this.loading = true;
      this.contactService.updateContact(this.formData).subscribe({
        next:(response: ApiResponse<string>) => {
          if(response.success) {
            alert('Contact updated successfully');
            this.loading = false;
            this.router.navigate(['/contacts']);
          } else {
            alert('Failed to update contact: ' + response.message);
            this.loading = false;
          }
        },
        error: (errorResponse) => {
          alert('Error while updating contact: ' + errorResponse.error.message);
          this.loading = false;
        }
      })
    } else {
      alert('Invalid model');
    }
  }
}
