import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SupportJacktimeComponent } from './support-jacktime.component';

describe('SupportJacktimeComponent', () => {
  let component: SupportJacktimeComponent;
  let fixture: ComponentFixture<SupportJacktimeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SupportJacktimeComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SupportJacktimeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
