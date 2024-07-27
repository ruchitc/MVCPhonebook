import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { LocalStorageService } from './helpers/local-storage.service';
import { User } from '../models/user.model';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { ApiResponse } from '../models/ApiResponse{T}';
import { LocalStorageKeys } from './helpers/localstoragekeys';
import { UserDetails } from '../models/user-details.model';
import { UpdatePassword } from '../models/update-password.model';
import { UserSecurityQuestions } from '../models/user-security-questions.model';
import { ResetPassword } from '../models/reset-password.model';
import { SecurityQuestion } from '../models/security-question.model';
import { UpdateUserDetails } from '../models/update-user-details.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = "http://localhost:5251/api/Auth/";
  private authState = new BehaviorSubject<boolean>(this.localStorageHelper.hasItem(LocalStorageKeys.TokenName));
  private usernameSubject = new BehaviorSubject<string | null | undefined>(this.localStorageHelper.getItem(LocalStorageKeys.UserId))

  constructor(private http: HttpClient, private localStorageHelper: LocalStorageService) { }

  signUp(user: User): Observable<ApiResponse<string>> {
    const body = user;
    return this.http.post<ApiResponse<string>>(this.apiUrl + "Register", body);
  }

  signIn(username: string, password: string): Observable<ApiResponse<string>> {
    const body = {username, password};
    return this.http.post<ApiResponse<string>>(this.apiUrl + 'Login', body).pipe(
      tap(response => {
        if(response.success) {
          this.localStorageHelper.setItem(LocalStorageKeys.TokenName, response.data);
          this.localStorageHelper.setItem(LocalStorageKeys.UserId, username);
          this.authState.next(true);
          this.usernameSubject.next(username);
        }
      })
    );
  }

  getUserDetails(username: string | null | undefined): Observable<ApiResponse<UserDetails>> {
    return this.http.get<ApiResponse<UserDetails>>(this.apiUrl + 'GetUserDetails/' + username);
  }

  updatePassword(updatePassword: UpdatePassword): Observable<ApiResponse<string>> {
    return this.http.put<ApiResponse<string>>(this.apiUrl + 'ChangePassword', updatePassword);
  }

  getSecurityQuestions(): Observable<ApiResponse<SecurityQuestion[]>> {
    return this.http.get<ApiResponse<SecurityQuestion[]>>(this.apiUrl + 'GetSecurityQuestions');
  }

  getUserSecurityQuestions(username: string | null | undefined): Observable<ApiResponse<UserSecurityQuestions>> {
    return this.http.get<ApiResponse<UserSecurityQuestions>>(this.apiUrl + 'GetUserSecurityQuestions/' + username);
  }

  resetPassword(resetPassword: ResetPassword): Observable<ApiResponse<string>> {
    return this.http.put<ApiResponse<string>>(this.apiUrl + 'ResetPassword', resetPassword);
  }

  updateUserDetails(updateUserDetails: UpdateUserDetails): Observable<ApiResponse<string>> {
    return this.http.put<ApiResponse<string>>(this.apiUrl + 'UpdateUserDetails', updateUserDetails).pipe(
      tap(response => {
        if(response.success) {
          this.localStorageHelper.setItem(LocalStorageKeys.TokenName, response.data);
          this.localStorageHelper.setItem(LocalStorageKeys.UserId, updateUserDetails.loginId);
          this.authState.next(true);
          this.usernameSubject.next(updateUserDetails.loginId);
        }
      })
    );
  }

  signOut() {
    this.localStorageHelper.removeItem(LocalStorageKeys.TokenName);
    this.localStorageHelper.removeItem(LocalStorageKeys.UserId);
    this.authState.next(false);
    this.usernameSubject.next(null);
  }

  isAuthenticated() {
    return this.authState.asObservable();
  }

  getUsername(): Observable<string | null | undefined> {
    return this.usernameSubject.asObservable();
  }
}
