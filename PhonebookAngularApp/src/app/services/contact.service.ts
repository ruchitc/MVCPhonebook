import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Contact } from '../models/contact.model';
import { ApiResponse } from '../models/ApiResponse{T}';
import { AddContact } from '../models/add-contact.model';
import { UpdateContact } from '../models/update-contact.model';
import { PaginationApiResponse } from '../models/PaginationApiResponse{T}';

@Injectable({
  providedIn: 'root'
})
export class ContactService {

  private apiUrl = "http://localhost:5251/api/Contact/";
  
  constructor(private http: HttpClient) { }

  getAllContacts(page: number, pageSize: number, searchString: string, sortDir: string, showFavourites: boolean): Observable<PaginationApiResponse<Contact[]>> {
    return this.http.get<PaginationApiResponse<Contact[]>>(this.apiUrl + 'GetAllContacts?' +
      'page=' + page +
      '&page_size=' + pageSize +
      '&search_string=' + searchString +
      '&sort_dir=' + sortDir +
      '&show_favourites=' + showFavourites
    );
  }

  getContactById(contactId: number): Observable<ApiResponse<Contact>> {
    return this.http.get<ApiResponse<Contact>>(this.apiUrl + 'GetContactById/' + contactId);
  }

  addContact(formData: FormData): Observable<ApiResponse<string>> {
    let headers = new HttpHeaders();
    headers.append('Content-Type', 'multipart/form-data');
    headers.append('Accept', 'application/json');

    return this.http.post<ApiResponse<string>>(this.apiUrl + 'AddContact', formData, {headers: headers});
  }

  updateContact(formData: FormData): Observable<ApiResponse<string>> {
    let headers = new HttpHeaders();
    headers.append('Content-Type', 'multipart/form-data');
    headers.append('Accept', 'application/json');

    return this.http.put<ApiResponse<string>>(this.apiUrl + 'UpdateContact', formData, {headers: headers});
  }

  deleteContact(contactId: number): Observable<ApiResponse<string>> {
    return this.http.delete<ApiResponse<string>>(this.apiUrl + 'DeleteContact/' + contactId);
  }
}
