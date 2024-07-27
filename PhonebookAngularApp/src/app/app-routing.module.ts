import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { SigninComponent } from './components/auth/signin/signin.component';
import { SignupComponent } from './components/auth/signup/signup.component';
import { HomeComponent } from './components/home/home.component';
import { ContactListComponent } from './components/contact/contact-list/contact-list.component';
import { PrivacyComponent } from './components/privacy/privacy.component';
import { SignupsuccessComponent } from './components/auth/signupsuccess/signupsuccess.component';
import { ContactDetailsComponent } from './components/contact/contact-details/contact-details.component';
import { AddContactComponent } from './components/contact/add-contact/add-contact.component';
import { UpdateContactComponent } from './components/contact/update-contact/update-contact.component';
import { authGuard } from './guards/auth.guard';
import { UserProfileComponent } from './components/user/user-profile/user-profile.component';
import { UpdatePasswordComponent } from './components/auth/update-password/update-password.component';
import { ForgotPasswordComponent } from './components/auth/forgot-password/forgot-password.component';
import { ResetPasswordComponent } from './components/auth/reset-password/reset-password.component';
import { UpdateUserDetailsComponent } from './components/auth/update-user-details/update-user-details.component';
import { AddContactRfComponent } from './components/contact/add-contact-rf/add-contact-rf.component';
import { UpdateContactRfComponent } from './components/contact/update-contact-rf/update-contact-rf.component';


const routes: Routes = [
  {path:'', redirectTo:'home', pathMatch:'full'},
  {path:'home', component:HomeComponent},
  {path:'privacy', component:PrivacyComponent},
  {path:'signin', component:SigninComponent},
  {path:'signup', component:SignupComponent},
  {path:'signupsuccess', component:SignupsuccessComponent},
  {path:'contacts', component:ContactListComponent},
  {path:'contacts/add', component:AddContactComponent, canActivate: [authGuard]},
  {path:'contacts/add-rf', component:AddContactRfComponent, canActivate: [authGuard]},
  {path:'contacts/details/:contactId', component:ContactDetailsComponent},
  {path:'contacts/edit/:contactId', component:UpdateContactComponent, canActivate: [authGuard]},
  {path:'contacts/edit-rf/:contactId', component:UpdateContactRfComponent, canActivate: [authGuard]},
  {path:'user/profile', component:UserProfileComponent, canActivate: [authGuard]},
  {path:'user/update-password', component:UpdatePasswordComponent, canActivate: [authGuard]},
  {path:'user/update-user-details', component:UpdateUserDetailsComponent, canActivate: [authGuard]},
  {path:'auth/forgot-password', component:ForgotPasswordComponent},
  {path:'auth/reset-password', component:ResetPasswordComponent},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
