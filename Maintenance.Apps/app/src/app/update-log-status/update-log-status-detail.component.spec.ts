import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UpdateLogStatusDetailComponent } from './update-log-status-detail.component';

describe('UpdateLogStatusDetailComponent', () => {
  let component: UpdateLogStatusDetailComponent;
  let fixture: ComponentFixture<UpdateLogStatusDetailComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ UpdateLogStatusDetailComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UpdateLogStatusDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
