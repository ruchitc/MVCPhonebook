import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomeComponent } from './components/home/home.component';
import { PrivacyComponent } from './components/privacy/privacy.component';
import { SigninComponent } from './components/auth/signin/signin.component';
import { SignupComponent } from './components/auth/signup/signup.component';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { ContactListComponent } from './components/contact/contact-list/contact-list.component';
import { SignupsuccessComponent } from './components/auth/signupsuccess/signupsuccess.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AuthService } from './services/auth.service';
import { AuthInterceptor } from './interceptors/auth.interceptor';
import { AddContactComponent } from './components/contact/add-contact/add-contact.component';
import { ContactDetailsComponent } from './components/contact/contact-details/contact-details.component';
import { UpdateContactComponent } from './components/contact/update-contact/update-contact.component';
import { NavbarComponent } from './components/shared/navbar/navbar.component';
import { FooterComponent } from './components/shared/footer/footer.component';
import { UserProfileComponent } from './components/user/user-profile/user-profile.component';
import { UpdatePasswordComponent } from './components/auth/update-password/update-password.component';
import { ForgotPasswordComponent } from './components/auth/forgot-password/forgot-password.component';
import { ResetPasswordComponent } from './components/auth/reset-password/reset-password.component';
import { UpdateUserDetailsComponent } from './components/auth/update-user-details/update-user-details.component';
import { AddContactRfComponent } from './components/contact/add-contact-rf/add-contact-rf.component';
import { UpdateContactRfComponent } from './components/contact/update-contact-rf/update-contact-rf.component';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    PrivacyComponent,
    SigninComponent,
    SignupComponent,
    ContactListComponent,
    SignupsuccessComponent,
    AddContactComponent,
    ContactDetailsComponent,
    UpdateContactComponent,
    NavbarComponent,
    FooterComponent,
    UserProfileComponent,
    UpdatePasswordComponent,
    ForgotPasswordComponent,
    ResetPasswordComponent,
    UpdateUserDetailsComponent,
    AddContactRfComponent,
    UpdateContactRfComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
  ],
  providers: [AuthService, {provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true}],
  bootstrap: [AppComponent]
})
export class AppModule { }
