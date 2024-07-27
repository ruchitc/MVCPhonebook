import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ApiResponse } from 'src/app/models/ApiResponse{T}';
import { Country } from 'src/app/models/country.model';
import { State } from 'src/app/models/state.model';
import { ContactService } from 'src/app/services/contact.service';
import { CountryService } from 'src/app/services/country.service';
import { StateService } from 'src/app/services/state.service';

@Component({
  selector: 'app-add-contact-rf',
  templateUrl: './add-contact-rf.component.html',
  styleUrls: ['./add-contact-rf.component.css']
})
export class AddContactRfComponent {
  countries: Country[] = [];
  states: State[] = [];
  addContactForm!: FormGroup;
  formData!: FormData;

  constructor(
    private contactService: ContactService,
    private countryService: CountryService,
    private stateService: StateService,
    private formBuilder: FormBuilder,
    private router: Router
  ) {

  }

  ngOnInit(): void {
    this.addContactForm = this.formBuilder.group({
      countryId: [0, [Validators.required, this.countryValidator]],
      stateId: [0, [Validators.required, this.stateValidator]],
      firstName: ['', [Validators.required, Validators.minLength(3)]],
      lastName: ['', [Validators.required, Validators.minLength(3)]],
      contactNumber: ['', [Validators.required, Validators.pattern("[0-9]{10}")]],
      email: ['', [Validators.required, Validators.email, Validators.maxLength(50)]],
      companyName: ['', [Validators.maxLength(50)]],
      gender: ['', [Validators.required]],
      isFavourite: [false],
      fileToUpload: [''],
      image: [''],
    })
    this.loadCountries();
  }

  countryValidator(control: any) {
    return control.value == '' ? {invalidCountry: true}: null;
  }

  stateValidator(control: any) {
    return control.value == '' ? {invalidState: true}: null;
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
          this.addContactForm.patchValue({
            stateId: this.states.length > 0 ? this.states[0].stateId : 0
          })
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
    this.addContactForm.patchValue({
      image: event.target.files[0]
    })
  }

  get formControls() {
    return this.addContactForm.controls;
  }

  onSubmit(): void {
    if (this.addContactForm.valid) {
      this.formData = new FormData;
    
    this.formData.append('FirstName', this.addContactForm.get('firstName')?.value);
    this.formData.append('LastName', this.addContactForm.get('lastName')?.value);
    this.formData.append('ContactNumber', this.addContactForm.get('contactNumber')?.value);
    this.formData.append('Gender', this.addContactForm.get('gender')?.value);
    this.formData.append('CountryId', this.addContactForm.get('countryId')?.value);
    this.formData.append('StateId', this.addContactForm.get('stateId')?.value);
    this.formData.append('IsFavourite', this.addContactForm.get('isFavourite')?.value);
    
    if(this.addContactForm.get('email')?.value == null) {
      this.formData.append('Email', '');
    }
    else {
      this.formData.append('Email', this.addContactForm.get('email')?.value);
    }

    if(this.addContactForm.get('companyName')?.value == null) {
      this.formData.append('Company', '');
    }
    else {
      this.formData.append('Company', this.addContactForm.get('companyName')?.value);
    }
    
    this.formData.forEach((value,key) => {
      console.log(key+" "+value)
    });
    this.formData.append('Image', this.addContactForm.get('image')?.value);
      this.contactService.addContact(this.formData).subscribe({
        next: (response) => {
          if(response.success) {
            this.router.navigate(['/contacts']);
          } else {
            alert(response.message);
          }
        },
        error: (err) => {
          alert('Failed to add contact: ' + err.error.message);
        }
      })
    }
  }
}
