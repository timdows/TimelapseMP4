import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { Mp4ListComponent } from './mp4-list.component';

describe('Mp4ListComponent', () => {
  let component: Mp4ListComponent;
  let fixture: ComponentFixture<Mp4ListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ Mp4ListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(Mp4ListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
