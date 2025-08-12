import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MapProviderDialogComponent } from './map-provider-dialog.component';

describe('MapProviderDialogComponent', () => {
  let component: MapProviderDialogComponent;
  let fixture: ComponentFixture<MapProviderDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MapProviderDialogComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MapProviderDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
