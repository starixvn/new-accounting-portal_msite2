import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientModule } from '@angular/common/http';
import { AngularMaterialModule } from './angular-material.module';
import { UpdateLogStatusComponent } from './update-log-status/update-log-status.component';
import { DeleteTempVariableNumberComponent } from './delete-temp-variable-number/delete-temp-variable-number.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { DeleteParlayLockingComponent } from './delete-parlay-locking/delete-parlay-locking.component';
import { DeleteSettlementLockComponent } from './delete-settlement-lock/delete-settlement-lock.component';
import { ConfirmDialogComponent } from './shared/components/confirm-dialog/confirm-dialog.component';
import { DeleteTempVariableNumberDetailComponent } from './delete-temp-variable-number/delete-temp-number-detail.component';
import { DeleteTempVariableNumberService } from './delete-temp-variable-number/delete-temp-variable-number.service';
import { HttpService } from './shared/services/http.service';
import { UpdateSystemSettingService } from './update-system-settings/update-system-settings.service';
import { UpdateSystemSettingsComponent } from './update-system-settings/update-system-settings.component';
import { SnackbarService } from './shared/components/snackbar/snackbar.service';
import { MatchSettlementLockService } from './delete-settlement-lock/match-settlement-lock.service';
import { MatchParlayLockingService } from './delete-parlay-locking/delete-parlay-locking.service';
import { ConfirmDialogService } from './shared/components/confirm-dialog/confirm-dialog.service';

@NgModule({
  declarations: [
    AppComponent,
    UpdateLogStatusComponent,
    DeleteTempVariableNumberComponent,
    NavMenuComponent,
    DeleteParlayLockingComponent,
    DeleteSettlementLockComponent,
    ConfirmDialogComponent,
    DeleteTempVariableNumberDetailComponent,
    UpdateSystemSettingsComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    AngularMaterialModule,
    BrowserAnimationsModule,
  ],
  providers: [
    HttpService,
    DeleteTempVariableNumberService,
    UpdateSystemSettingService,
    MatchSettlementLockService,
    SnackbarService,
    MatchParlayLockingService,
    ConfirmDialogService,
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
