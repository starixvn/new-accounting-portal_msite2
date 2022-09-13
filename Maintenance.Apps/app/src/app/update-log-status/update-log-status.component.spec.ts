import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UpdateLogStatusComponent } from './update-log-status.component';

describe('UpdateLogStatusComponent', () => {
  let component: UpdateLogStatusComponent;
  let fixture: ComponentFixture<UpdateLogStatusComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ UpdateLogStatusComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UpdateLogStatusComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
