import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UpdateContactComponent } from './update-contact.component';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { FormsModule } from '@angular/forms';
import { RouterTestingModule } from '@angular/router/testing';

describe('UpdateContactComponent', () => {
  let component: UpdateContactComponent;
  let fixture: ComponentFixture<UpdateContactComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule, RouterTestingModule, FormsModule],
      declarations: [UpdateContactComponent]
    });
    fixture = TestBed.createComponent(UpdateContactComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
