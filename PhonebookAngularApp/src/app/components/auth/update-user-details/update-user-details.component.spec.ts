import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UpdateUserDetailsComponent } from './update-user-details.component';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { FormsModule } from '@angular/forms';
import { RouterTestingModule } from '@angular/router/testing';

describe('UpdateUserDetailsComponent', () => {
  let component: UpdateUserDetailsComponent;
  let fixture: ComponentFixture<UpdateUserDetailsComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule, RouterTestingModule, FormsModule],
      declarations: [UpdateUserDetailsComponent]
    });
    fixture = TestBed.createComponent(UpdateUserDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
