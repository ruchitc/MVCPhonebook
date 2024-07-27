import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Form, NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { ApiResponse } from 'src/app/models/ApiResponse{T}';
import { AddContact } from 'src/app/models/add-contact.model';
import { Country } from 'src/app/models/country.model';
import { State } from 'src/app/models/state.model';
import { ContactService } from 'src/app/services/contact.service';
import { CountryService } from 'src/app/services/country.service';
import { StateService } from 'src/app/services/state.service';

@Component({
  selector: 'app-add-contact',
  templateUrl: './add-contact.component.html',
  styleUrls: ['./add-contact.component.css']
})
export class AddContactComponent implements OnInit {
  loading: boolean = false;
  contact: AddContact = {
    firstName: '',
    lastName: '',
    contactNumber: '',
    gender: '',
    countryId: 0,
    stateId: 0,
    isFavourite: false,
    email: '',
    company: '',
    image: null,
  }

  formData!: FormData;

  countries: Country[] = [];
  states: State[] = [];

  constructor(
    private contactService: ContactService,
    private countryService: CountryService,
    private stateService: StateService,
    private router: Router) {
    
  }

  ngOnInit(): void {
    this.loadCountries();
  }

  loadCountries(): void {
    this.countryService.getAllCountries().subscribe({
      next:(response: ApiResponse<Country[]>) => {
        if(response.success) {
          this.countries = response.data;
          this.countries.sort((a, b) => 
            a.countryName > b.countryName ? 1 : -1);
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
          this.states.sort((a, b) =>
            a.stateName > b.stateName ? 1 : -1);
          this.contact.stateId = this.states[0].stateId;
        } else {
          console.error('Failed to fetch states', response.message)
        }
      },
      error: (errorResponse) => {
        console.error('Error fetching states: ',  errorResponse.error.message);
      }
    })
  }

  handleFileInput(event: any) {
    this.contact.image = event.target.files[0];
  }

  onSubmit(addContactForm: NgForm) {
    this.formData = new FormData;
    
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

    if(addContactForm.valid) {
      this.loading = true;
      this.contactService.addContact(this.formData).subscribe({
        next:(response: ApiResponse<string>) => {
          if(response.success) {
            alert('Contact added successfully');
            this.loading = false;
            this.router.navigate(['/contacts']);
          } else {
            alert('Failed to add contact: ' + response.message);
            this.loading = false;
          }
        },
        error: (errorResponse) => {
          alert('Error while adding contact: ' + errorResponse.error.message);
          this.loading = false;
        }
      })
    } else {
      alert('Invalid model');
    }
  }
}
