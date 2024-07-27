import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';

import { ContactService } from './contact.service';
import { ApiResponse } from '../models/ApiResponse{T}';
import { Contact } from '../models/contact.model';
import { AddContact } from '../models/add-contact.model';
import { UpdateContact } from '../models/update-contact.model';
import { concatAll } from 'rxjs';

describe('ContactService', () => {
  let service: ContactService;
  let httpMock: HttpTestingController;
  const mockApiResponse: ApiResponse<Contact[]> = {
    success: true,
    data: [
      {
        contactId: 1,
        firstName: 'Test',
        lastName: 'Test',
        contactNumber: '1234567890',
        gender: 'M',
        countryId: 1,
        stateId: 1,
        isFavourite: true,
        email: 'test@example.com',
        company: 'Example',
        fileName: 'test.png',
        country: {
          countryId: 1,
          countryName: 'Test'
        },
        state: {
          stateId: 1,
          stateName: 'Test',
          countryId: 1
        },
        imageBytes: undefined
      },
      {
        contactId: 2,
        firstName: 'Test2',
        lastName: 'Test2',
        contactNumber: '1234567891',
        gender: 'F',
        countryId: 2,
        stateId: 2,
        isFavourite: true,
        email: 'test2@example.com',
        company: 'Example',
        fileName: 'test.png',
        country: {
          countryId: 2,
          countryName: 'Test2'
        },
        state: {
          stateId: 2,
          stateName: 'Test2',
          countryId: 2
        },
        imageBytes: undefined
      }
    ],
    message: ''
  }

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [ContactService]
    });
    service = TestBed.inject(ContactService);
    httpMock = TestBed.inject(HttpTestingController)
  });

  afterEach(() => {
    httpMock.verify();
  })

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should fetch all contacts successfully', () => {
    // Arrange
    let page: number = 1;
    let pageSize: number = 10;
    let searchString: string = '';
    let sortDir: string = 'default';
    let showFavourites: boolean = false;

    const apiUrl = "http://localhost:5251/api/Contact/GetAllContacts?" +
    'page=' + page +
    '&page_size=' + pageSize +
    '&search_string=' + searchString +
    '&sort_dir=' + sortDir +
    '&show_favourites=' + showFavourites;

    // Act
    service.getAllContacts(page, pageSize, searchString, sortDir, showFavourites).subscribe((response) => {
      // Assert
      expect(response.data.length).toBe(2);
      expect(response.data).toEqual(mockApiResponse.data);
    });

    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('GET');
    req.flush(mockApiResponse);
  })

  it('should handle an empty contacts list', () => {
    // Arrange
    let page: number = 1;
    let pageSize: number = 10;
    let searchString: string = '';
    let sortDir: string = 'default';
    let showFavourites: boolean = false;

    const apiUrl = "http://localhost:5251/api/Contact/GetAllContacts?" +
    'page=' + page +
    '&page_size=' + pageSize +
    '&search_string=' + searchString +
    '&sort_dir=' + sortDir +
    '&show_favourites=' + showFavourites;
    
    const emptyResponse: ApiResponse<Contact[]> = {
      success: true,
      data: [],
      message: ''
    }

    // Act
    service.getAllContacts(page, pageSize, searchString, sortDir, showFavourites).subscribe((response) => {
      // Assert
      expect(response.data.length).toBe(0);
      expect(response.data).toEqual([]);
    });

    const req = httpMock.expectOne(apiUrl)
    expect(req.request.method).toBe('GET');
    req.flush(emptyResponse);
  });

  it('should handle HTTP error gracefully', () => {
    let page: number = 1;
    let pageSize: number = 10;
    let searchString: string = '';
    let sortDir: string = 'default';
    let showFavourites: boolean = false;

    const apiUrl = "http://localhost:5251/api/Contact/GetAllContacts?" +
    'page=' + page +
    '&page_size=' + pageSize +
    '&search_string=' + searchString +
    '&sort_dir=' + sortDir +
    '&show_favourites=' + showFavourites;

    const errorMessage = "Failed to load contacts";

    // Act
    service.getAllContacts(page, pageSize, searchString, sortDir, showFavourites).subscribe(
      () => fail('expected an error, not contacts'),
      (error) => {
        // Assert
        expect(error.status).toBe(500);
        expect(error.statusText).toBe('Internal Server Error');
      }
    );

    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('GET');

    // Respond with error
    req.flush(errorMessage, {status: 500, statusText: 'Internal Server Error'});
  });

  it('should fetch contact by id successfully', () => {
    // Arrange
    const contactId = 1;
    const mockSuccessResponse: ApiResponse<Contact> = {
      success: true,
      data: {
        contactId: 1,
        firstName: 'FirstName',
        lastName: 'LastName',
        contactNumber: '1234567890',
        gender: 'M',
        countryId: 1,
        stateId: 1,
        isFavourite: false,
        email: 'test@example.com',
        company: 'Test',
        fileName: 'file.jpg',
        country: {
          countryId: 1,
          countryName: 'CountryName'
        },
        state: {
          stateId: 1,
          stateName: 'StateName',
          countryId: 1
        },
        imageBytes: undefined
      },
      message: ''
    };

    // Act
    service.getContactById(contactId).subscribe(response => {
      // Assert
      expect(response.success).toBeTrue();
      expect(response.message).toBe('');
      expect(response.data).toBe(mockSuccessResponse.data);
      expect(response.data.contactId).toBe(contactId);
    });

    const req = httpMock.expectOne('http://localhost:5251/api/Contact/GetContactById/' + contactId);
    expect(req.request.method).toBe('GET');
    req.flush(mockSuccessResponse);
  });

  it('should handle failed contact retrieval', () => {
    // Arrange
    const contactId = 1;
    const mockErrorResponse: ApiResponse<Contact> = {
      success: false,
      data: {} as Contact,
      message: 'No record found'
    };

    // Act
    service.getContactById(contactId).subscribe(response => {
      // Assert
      expect(response.success).toBeFalse();
      expect(response.message).toBe('No record found');
      expect(response.data).toBe(mockErrorResponse.data);
    });

    const req = httpMock.expectOne('http://localhost:5251/api/Contact/GetContactById/' + contactId);
    expect(req.request.method).toBe('GET');
    req.flush(mockErrorResponse);
  });

  it('should handle http error when retrieval fails', () => {
    // Arrange
    const contactId = 1;
    const mockHttpError = {
      status: 500,
      statusText: 'Internal Server Error',
    };

    // Act
    service.getContactById(contactId).subscribe({
      next: () => fail ('should have failed with 500 error'),
      error: (error) => {
        // Assert
        expect(error.status).toBe(500);
        expect(error.statusText).toBe('Internal Server Error');
      }
    });

    const req = httpMock.expectOne('http://localhost:5251/api/Contact/GetContactById/' + contactId);
    expect(req.request.method).toBe('GET');
    req.flush({}, mockHttpError);
  });

  it('should add a contact successfully', () => {
    // Arrange
    const addContact: AddContact = {
      firstName: 'FirstName',
      lastName: 'LastName',
      contactNumber: '1234567890',
      gender: 'M',
      countryId: 1,
      stateId: 1,
      isFavourite: false,
      email: 'test@example.com',
      company: 'Test',
      image: undefined
    };

    const formData = new FormData;
    formData.append('FirstName', addContact.firstName);
    formData.append('LastName', addContact.lastName);
    formData.append('ContactNumber', addContact.contactNumber);
    formData.append('Gender', addContact.gender);
    formData.append('CountryId', addContact.countryId.toString());
    formData.append('StateId', addContact.stateId.toString());
    formData.append('IsFavourite', addContact.isFavourite.toString());
    formData.append('Email', addContact.email);
    formData.append('Company', addContact.company);
    formData.append('Image', addContact.image);

    const mockSuccessResponse: ApiResponse<string> = {
      data: '',
      success: true,
      message: 'Contact saved successfully'
    };

    // Act
    service.addContact(formData).subscribe(response => {
      // Assert
      expect(response).toBe(mockSuccessResponse);
      expect(response.message).toBe('Contact saved successfully');
      expect(response.success).toBeTrue();
    });

    const req = httpMock.expectOne('http://localhost:5251/api/Contact/AddContact');
    expect(req.request.method).toBe('POST');
    req.flush(mockSuccessResponse);
  });

  it('should handle failed contact addition', () => {
    // Arrange
    const addContact: AddContact = {
      firstName: 'FirstName',
      lastName: 'LastName',
      contactNumber: '1234567890',
      gender: 'M',
      countryId: 1,
      stateId: 1,
      isFavourite: false,
      email: 'test@example.com',
      company: 'Test',
      image: undefined
    };

    const formData = new FormData;
    formData.append('FirstName', addContact.firstName);
    formData.append('LastName', addContact.lastName);
    formData.append('ContactNumber', addContact.contactNumber);
    formData.append('Gender', addContact.gender);
    formData.append('CountryId', addContact.countryId.toString());
    formData.append('StateId', addContact.stateId.toString());
    formData.append('IsFavourite', addContact.isFavourite.toString());
    formData.append('Email', addContact.email);
    formData.append('Company', addContact.company);
    formData.append('Image', addContact.image);

    const mockErrorResponse: ApiResponse<string> = {
      data: '',
      success: false,
      message: 'Contact already exists'
    };

    // Act
    service.addContact(formData).subscribe(response => {
      // Assert
      expect(response).toBe(mockErrorResponse);
      expect(response.message).toBe('Contact already exists');
      expect(response.success).toBeFalse();
    });
    const req = httpMock.expectOne('http://localhost:5251/api/Contact/AddContact');
    expect(req.request.method).toBe('POST');
    req.flush(mockErrorResponse);
  });

  it('should handle http error when creation fails', () => {
    // Arrange
    const addContact: AddContact = {
      firstName: 'FirstName',
      lastName: 'LastName',
      contactNumber: '1234567890',
      gender: 'M',
      countryId: 1,
      stateId: 1,
      isFavourite: false,
      email: 'test@example.com',
      company: 'Test',
      image: undefined
    };

    const formData = new FormData;
    formData.append('FirstName', addContact.firstName);
    formData.append('LastName', addContact.lastName);
    formData.append('ContactNumber', addContact.contactNumber);
    formData.append('Gender', addContact.gender);
    formData.append('CountryId', addContact.countryId.toString());
    formData.append('StateId', addContact.stateId.toString());
    formData.append('IsFavourite', addContact.isFavourite.toString());
    formData.append('Email', addContact.email);
    formData.append('Company', addContact.company);
    formData.append('Image', addContact.image);

    const mockHttpError = {
      status: 500,
      statusText: 'Internal Server Error'
    };

    // Act
    service.addContact(formData).subscribe({
      next: () => fail ('should have failed with the 500 error'),
      error: (error) => {
        // Assert
        expect(error.status).toEqual(500);
        expect(error.statusText).toEqual('Internal Server Error');
      }
    });

    const req = httpMock.expectOne('http://localhost:5251/api/Contact/AddContact');
    expect(req.request.method).toBe('POST');
    req.flush({}, mockHttpError);
  });

  it('should update a contact successfully', () => {
    // Arrange
    const updatedContact: UpdateContact = {
      contactId: 1,
      firstName: 'FirstName',
      lastName: 'LastName',
      contactNumber: '1234567890',
      gender: 'M',
      countryId: 1,
      stateId: 1,
      isFavourite: false,
      email: 'test@example.com',
      company: 'Test',
      image: undefined
    };

    const formData = new FormData;
    formData.append('ContactId', updatedContact.contactId.toString());
    formData.append('FirstName', updatedContact.firstName);
    formData.append('LastName', updatedContact.lastName);
    formData.append('ContactNumber', updatedContact.contactNumber);
    formData.append('Gender', updatedContact.gender);
    formData.append('CountryId', updatedContact.countryId.toString());
    formData.append('StateId', updatedContact.stateId.toString());
    formData.append('IsFavourite', updatedContact.isFavourite.toString());
    formData.append('Email', updatedContact.email);
    formData.append('Company', updatedContact.company);
    formData.append('Image', updatedContact.image);
    
    const mockSuccessResponse: ApiResponse<string> = {
      data: '',
      success: true,
      message: 'Contact updated successfully.'
    };
    // Act
    service.updateContact(formData).subscribe(
      response => {
        // Assert
        expect(response).toEqual(mockSuccessResponse);
      }
    );
    const req = httpMock.expectOne('http://localhost:5251/api/Contact/UpdateContact');
    expect(req.request.method).toBe('PUT');
    req.flush(mockSuccessResponse);
  });

  it('should handle failed contact modification', () => {
    // Arrange
    const updatedContact: UpdateContact = {
      contactId: 1,
      firstName: 'FirstName',
      lastName: 'LastName',
      contactNumber: '1234567890',
      gender: 'M',
      countryId: 1,
      stateId: 1,
      isFavourite: false,
      email: 'test@example.com',
      company: 'Test',
      image: undefined
    };

    const formData = new FormData;
    formData.append('ContactId', updatedContact.contactId.toString());
    formData.append('FirstName', updatedContact.firstName);
    formData.append('LastName', updatedContact.lastName);
    formData.append('ContactNumber', updatedContact.contactNumber);
    formData.append('Gender', updatedContact.gender);
    formData.append('CountryId', updatedContact.countryId.toString());
    formData.append('StateId', updatedContact.stateId.toString());
    formData.append('IsFavourite', updatedContact.isFavourite.toString());
    formData.append('Email', updatedContact.email);
    formData.append('Company', updatedContact.company);
    formData.append('Image', updatedContact.image);
    
    const mockErrorResponse: ApiResponse<string> = {
      data: '',
      success: false,
      message: 'Failed to update contact.'
    };

    // Act
    service.updateContact(formData).subscribe(
      response => {
        // Assert
        expect(response).toEqual(mockErrorResponse);
      }
    );
    const req = httpMock.expectOne('http://localhost:5251/api/Contact/UpdateContact');
    expect(req.request.method).toBe('PUT');
    req.flush(mockErrorResponse);
  });

  it('should handle http error when modification fails', () => {
    // Arrange
    const updatedContact: UpdateContact = {
      contactId: 1,
      firstName: 'FirstName',
      lastName: 'LastName',
      contactNumber: '1234567890',
      gender: 'M',
      countryId: 1,
      stateId: 1,
      isFavourite: false,
      email: 'test@example.com',
      company: 'Test',
      image: undefined
    };

    const formData = new FormData;
    formData.append('ContactId', updatedContact.contactId.toString());
    formData.append('FirstName', updatedContact.firstName);
    formData.append('LastName', updatedContact.lastName);
    formData.append('ContactNumber', updatedContact.contactNumber);
    formData.append('Gender', updatedContact.gender);
    formData.append('CountryId', updatedContact.countryId.toString());
    formData.append('StateId', updatedContact.stateId.toString());
    formData.append('IsFavourite', updatedContact.isFavourite.toString());
    formData.append('Email', updatedContact.email);
    formData.append('Company', updatedContact.company);
    formData.append('Image', updatedContact.image);
    
    const mockHttpErrorResponse = {
      status: 500,
      statusText: 'Internal Server Error'
    };

    // Act
    service.updateContact(formData).subscribe({
      next: () => fail ('should have failed with the 500 error'),
      error: (error) => {
        // Assert
        expect(error.status).toEqual(500);
        expect(error.statusText).toEqual('Internal Server Error');
      }
    });

    const req = httpMock.expectOne('http://localhost:5251/api/Contact/UpdateContact');
    expect(req.request.method).toBe('PUT');
    req.flush({}, mockHttpErrorResponse);
  });

  it('should delete a contact by id successfully', () => {
    // Arrange
    const contactId = 1;
    const mockSuccessResponse: ApiResponse<string> = {
      data: '',
      success: true,
      message: 'Contact deleted successfully'
    };

    // Act
    service.deleteContact(contactId).subscribe(response => {
      // Assert
      expect(response).toEqual(mockSuccessResponse);
      expect(response.success).toBeTrue;
      expect(response.message).toEqual('Contact deleted successfully');
    });

    const req = httpMock.expectOne('http://localhost:5251/api/Contact/DeleteContact/' + contactId);
    expect(req.request.method).toBe('DELETE');
    req.flush(mockSuccessResponse);
  });

  it('should handle failed contact deletion', () => {
    // Arrange
    const contactId = 1;
    const mockErrorResponse: ApiResponse<string> = {
      data: '',
      success: false,
      message: 'Failed to delete contact'
    };

    // Act
    service.deleteContact(contactId).subscribe(response => {
      // Assert
      expect(response).toEqual(mockErrorResponse);
      expect(response.success).toBeFalse;
      expect(response.message).toEqual('Failed to delete contact');
    });

    const req = httpMock.expectOne('http://localhost:5251/api/Contact/DeleteContact/' + contactId);
    expect(req.request.method).toBe('DELETE');
    req.flush(mockErrorResponse);
  });

  it('should handle http error when deletion fails', () => {
    // Arrange
    const contactId = 1;
    const mockHttpError = {
      status: 500,
      statusText: 'Internal Server Error',
    };
    
    // Act
    service.deleteContact(contactId).subscribe({
      next: () => fail ('should have failed with 500 error'),
      error: (error) => {
        // Assert
        expect(error.status).toBe(500);
        expect(error.statusText).toBe('Internal Server Error');
      }
    });

    const req = httpMock.expectOne('http://localhost:5251/api/Contact/DeleteContact/' + contactId);
    expect(req.request.method).toBe('DELETE');
    req.flush({}, mockHttpError);
  });
});
