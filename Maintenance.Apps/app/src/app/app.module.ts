import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientModule } from '@angular/common/http';
import { AngularMaterialModule } from './angular-material.module';
import { UpdateLogStatusComponent } from './update-log-status/update-log-status.component';
import { DeleteTempVariableNumberComponent } from './delete-temp-variable-number/delete-temp-variable-number.component';
import { HttpService } from 'src/services/http.service';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { DeleteParlayLockingComponent } from './delete-parlay-locking/delete-parlay-locking.component';
import { DeleteSettlementLockComponent } from './delete-settlement-lock/delete-settlement-lock.component';
import { ConfirmDialogComponent } from './shared/confirm-dialog/confirm-dialog.component';
import { DeleteTempVariableNumberDetailComponent } from './delete-temp-variable-number/delete-temp-number-detail.component';
import { Service } from './delete-temp-variable-number/delete-temp-variable-number.service';

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
  providers: [HttpService, Service],
  bootstrap: [AppComponent],
})
export class AppModule {}
