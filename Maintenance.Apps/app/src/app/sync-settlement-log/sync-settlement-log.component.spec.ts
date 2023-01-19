import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SyncSettlementLogComponent } from './sync-settlement-log.component';

describe('SyncSettlementLogToolComponent', () => {
    let component: SyncSettlementLogComponent;
    let fixture: ComponentFixture<SyncSettlementLogComponent>;

    beforeEach(async () => {
        await TestBed.configureTestingModule({
            declarations: [SyncSettlementLogComponent]
        })
            .compileComponents();

        fixture = TestBed.createComponent(SyncSettlementLogComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
