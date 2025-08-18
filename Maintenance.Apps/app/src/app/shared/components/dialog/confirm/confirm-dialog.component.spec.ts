import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ConfirmDialogComponent } from './confirm-dialog.component';

import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { ConfirmDialogData } from './confirm-dialog.model';

describe('ConfirmDialogComponent', () => {
  let component: ConfirmDialogComponent;
  let fixture: ComponentFixture<ConfirmDialogComponent>;
  let dialogRefSpy: any;
  const dialogData: ConfirmDialogData = {
    title: 'Test Title',
    message: 'Test Message',
    confirmText: 'Yes',
    cancelText: 'No',
    confirmStyle: 'custom-ok',
    cancelStyle: 'custom-cancel',
  };

  beforeEach(() => {
    dialogRefSpy = { close: jasmine.createSpy('close') };
    TestBed.configureTestingModule({
      declarations: [ConfirmDialogComponent],
      providers: [
        { provide: MatDialogRef, useValue: dialogRefSpy },
        { provide: MAT_DIALOG_DATA, useValue: dialogData },
      ],
    });
    fixture = TestBed.createComponent(ConfirmDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize dialog data with provided values', () => {
    expect(component.title).toBe(dialogData.title);
    expect(component.message).toBe(dialogData.message);
    expect(component.confirmText).toBe(dialogData.confirmText);
    expect(component.cancelText).toBe(dialogData.cancelText);
    expect(component.confirmStyle).toBe(dialogData.confirmStyle);
    expect(component.cancelStyle).toBe(dialogData.cancelStyle);
  });

  it('should use default values if not provided', () => {
    const defaultData: ConfirmDialogData = { message: 'msg' };
    TestBed.overrideProvider(MAT_DIALOG_DATA, { useValue: defaultData });
    const defaultFixture = TestBed.createComponent(ConfirmDialogComponent);
    const defaultComponent = defaultFixture.componentInstance;
    defaultFixture.detectChanges();
    expect(defaultComponent.title).toBe('Please confirm');
    expect(defaultComponent.confirmText).toBe('Ok');
    expect(defaultComponent.cancelText).toBe('Cancel');
    expect(defaultComponent.confirmStyle).toBe('btn-confirm-ok');
    expect(defaultComponent.cancelStyle).toBe('btn-confirm-cancel');
  });

  it('should close dialog with true onOk', () => {
    component.onOk();
    expect(dialogRefSpy.close).toHaveBeenCalledWith(true);
  });

  it('should close dialog with false onCancel', () => {
    component.onCancel();
    expect(dialogRefSpy.close).toHaveBeenCalledWith(false);
  });
});
