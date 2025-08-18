import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { WarningDialogComponent } from './warning-dialog.component';

describe('WarningDialogComponent', () => {
  let component: WarningDialogComponent;
  let fixture: ComponentFixture<WarningDialogComponent>;
  let dialogRefSpy: any;
  const dialogData = { message: 'Test warning message' };

  beforeEach(() => {
    dialogRefSpy = { close: jasmine.createSpy('close') };
    TestBed.configureTestingModule({
      declarations: [WarningDialogComponent],
      providers: [
        { provide: MatDialogRef, useValue: dialogRefSpy },
        { provide: MAT_DIALOG_DATA, useValue: dialogData },
      ],
    });
    fixture = TestBed.createComponent(WarningDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize message from dialog data', () => {
    expect(component.message).toBe(dialogData.message);
  });

  it('should close dialog with true onOk', () => {
    component.onOk();
    expect(dialogRefSpy.close).toHaveBeenCalledWith(true);
  });
});
