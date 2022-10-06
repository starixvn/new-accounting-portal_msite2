import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DeleteTempVariableNumberComponent } from './delete-temp-variable-number.component';

describe('DeleteTempVariableNumberComponent', () => {
  let component: DeleteTempVariableNumberComponent;
  let fixture: ComponentFixture<DeleteTempVariableNumberComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [DeleteTempVariableNumberComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(DeleteTempVariableNumberComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
