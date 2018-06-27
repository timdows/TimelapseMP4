import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { Hour1400ImagesComponent } from './hour1400-images.component';

describe('Hour1400ImagesComponent', () => {
  let component: Hour1400ImagesComponent;
  let fixture: ComponentFixture<Hour1400ImagesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ Hour1400ImagesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(Hour1400ImagesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
