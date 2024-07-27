import { Component } from '@angular/core';
import { FormGroup, FormBuilder, Validators, NgForm } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ApiResponse } from 'src/app/models/ApiResponse{T}';
import { Contact } from 'src/app/models/contact.model';
import { Country } from 'src/app/models/country.model';
import { State } from 'src/app/models/state.model';
import { ContactService } from 'src/app/services/contact.service';
import { CountryService } from 'src/app/services/country.service';
import { StateService } from 'src/app/services/state.service';

@Component({
  selector: 'app-update-contact-rf',
  templateUrl: './update-contact-rf.component.html',
  styleUrls: ['./update-contact-rf.component.css']
})
export class UpdateContactRfComponent {
  countries: Country[] = [];
  states: State[] = [];
  updateContactForm!: FormGroup;
  formData!: FormData;

  constructor(
    private contactService: ContactService,
    private countryService: CountryService,
    private stateService: StateService,
    private formBuilder: FormBuilder,
    private router: Router,
    private currentRoute: ActivatedRoute
  ) {

  }

  ngOnInit(): void {
    this.updateContactForm = this.formBuilder.group({
      contactId: [0],
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
    const contactId = this.currentRoute.snapshot.paramMap.get('contactId');
    this.loadContactDetails(Number(contactId));
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
          this.updateContactForm.patchValue({
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

  loadContactDetails(contactId: number): void {
    this.contactService.getContactById(contactId).subscribe({
      next:(response: ApiResponse<Contact>) => {
        if(response.success) {
          this.updateContactForm.setValue({
            contactId: response.data.contactId,
            firstName: response.data.firstName,
            lastName: response.data.lastName,
            contactNumber: response.data.contactNumber,
            gender: response.data.gender,
            countryId: response.data.countryId,
            stateId: response.data.stateId,
            isFavourite: response.data.isFavourite,
            email: response.data.email,
            companyName: response.data.company,
            image: response.data.fileName,
            fileToUpload: ''
          })
          // // Load states only after contact details are loaded
          // this.initialStateId = response.data.stateId;
          this.loadStatesByCountryId(response.data.countryId);
        } else {
          alert('Failed to get contact: ' + response.message);
        }
      },
      error:(errorResponse) => {
        alert('Error: ' + errorResponse.error.message);
        this.router.navigate(['/contacts']);
      }
    })
  }

  handleFileInput(event: any) {
    this.updateContactForm.patchValue({
      image: event.target.files[0]
    })
  }

  get formControls() {
    return this.updateContactForm.controls;
  }

  onSubmit(): void {
    if (this.updateContactForm.valid) {
      this.formData = new FormData;
    
      this.formData.append('ContactId', this.updateContactForm.get('contactId')?.value);
    this.formData.append('FirstName', this.updateContactForm.get('firstName')?.value);
    this.formData.append('LastName', this.updateContactForm.get('lastName')?.value);
    this.formData.append('ContactNumber', this.updateContactForm.get('contactNumber')?.value);
    this.formData.append('Gender', this.updateContactForm.get('gender')?.value);
    this.formData.append('CountryId', this.updateContactForm.get('countryId')?.value);
    this.formData.append('StateId', this.updateContactForm.get('stateId')?.value);
    this.formData.append('IsFavourite', this.updateContactForm.get('isFavourite')?.value);
    
    if(this.updateContactForm.get('email')?.value == null) {
      this.formData.append('Email', '');
    }
    else {
      this.formData.append('Email', this.updateContactForm.get('email')?.value);
    }

    if(this.updateContactForm.get('companyName')?.value == null) {
      this.formData.append('Company', '');
    }
    else {
      this.formData.append('Company', this.updateContactForm.get('companyName')?.value);
    }
    
    this.formData.forEach((value,key) => {
      console.log(key+" "+value)
    });
    this.formData.append('Image', this.updateContactForm.get('image')?.value);
      this.contactService.updateContact(this.formData).subscribe({
        next: (response) => {
          if(response.success) {
            this.router.navigate(['/contacts']);
          } else {
            alert(response.message);
          }
        },
        error: (err) => {
          alert('Failed to update contact: ' + err.error.message);
        }
      })
    }
  }
}
