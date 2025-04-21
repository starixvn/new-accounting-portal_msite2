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
import { DeleteTempVariableNumberDetailComponent } from './delete-temp-variable-number/delete-temp-number-detail.component';
import { DeleteTempVariableNumberService } from './delete-temp-variable-number/delete-temp-variable-number.service';
import { HttpService } from './shared/services/http.service';
import { UpdateSystemSettingService } from './update-system-settings/update-system-settings.service';
import { UpdateSystemSettingsComponent } from './update-system-settings/update-system-settings.component';
import { SnackbarService } from './shared/services/snackbar.service';
import { BlockUIService } from './shared/services/block-ui.service';
import { ReplaceLineBreaks } from './shared/pipes/replace-line-breaks';
import { MatchSettlementLockService } from './delete-settlement-lock/match-settlement-lock.service';
import { MatchParlayLockingService } from './delete-parlay-locking/delete-parlay-locking.service';
import { ConfirmDialogService } from './shared/components/dialog/dialog.service';
import { NavMenuService } from './nav-menu/nav-menu.service';
import { UpdateLogStatusService } from './update-log-status/update-log-status.service';
import { UpdateLogStatusDetailComponent } from './update-log-status/update-log-status-detail.component';
import { ClipboardModule } from '@angular/cdk/clipboard';
import { PasteIconComponent } from './update-log-status/paste-icon.component';
import { ControlSyncServiceComponent } from './control-sync-service/control-sync-service.component';
import { ControlSyncService } from './control-sync-service/control-sync-service.service';
import { SyncSettlementLogComponent } from './sync-settlement-log/sync-settlement-log.component';
import { SettlementErrorLogService } from './sync-settlement-log/sync-settlement-log.service';
import { DetailsSettlementLogComponent } from './sync-settlement-log/details-settlement-log.component';
import { ConfirmDialogComponent } from './shared/components/dialog/confirm/confirm-dialog.component';
import { WarningDialogComponent } from './shared/components/dialog/warning/warning-dialog.component';
import { FileService } from './shared/services/file.service';
import { SyncTransidDirectlyComponent } from './sync-transid-directly/sync-transid-directly.component';
import { SyncTransIdService } from './sync-transid-directly/sync-transid-directly.service';
import { UpdateFlagBroadcastComponent } from './update-flag-broadcast/update-flag-broadcast.component';
import { UpdateFlagRebateRetentionComponent } from './update-flag-rebate-retention/update-flag-rebate-retention.component';
import { UpdateFlagBroadcastService } from './update-flag-broadcast/update-flag-broadcast.service';
import { UpdateFlagRebateRetentionService } from './update-flag-rebate-retention/update-flag-rebate-retention.service';
import { ResultAmendmentCompoment } from './result-amendment/result-amendment.compoment';
import { ResultAmendmentService } from './result-amendment/result-amendment.service';
import { NameStatus } from './shared/pipes/name-status';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatFormFieldModule } from '@angular/material/form-field';
import { LeagueFilterComponent } from './special-league-setting/league-filter.component';
import { LeagueSettingComponent } from './special-league-setting/league-setting.component';
import { LeagueDataComponent } from './special-league-setting/league-data.component';
import { LeagueSettingService } from './special-league-setting/league-setting.service';
import { UpdateFlagSitedbComponent } from './update-flag-sitedb/update-flag-sitedb.component';
import { UpdateFlagSitedbService } from './update-flag-sitedb/update-flag-sitedb.service';

@NgModule({
  declarations: [
    AppComponent,
    UpdateLogStatusComponent,
    DeleteTempVariableNumberComponent,
    NavMenuComponent,
    DeleteParlayLockingComponent,
    DeleteSettlementLockComponent,
    ConfirmDialogComponent,
    WarningDialogComponent,
    DeleteTempVariableNumberDetailComponent,
    UpdateSystemSettingsComponent,
    UpdateLogStatusDetailComponent,
    ControlSyncServiceComponent,
    PasteIconComponent,
    ReplaceLineBreaks,
    SyncSettlementLogComponent,
    DetailsSettlementLogComponent,
    SyncTransidDirectlyComponent,
    UpdateFlagBroadcastComponent,
    UpdateFlagRebateRetentionComponent,
    ResultAmendmentCompoment,
    LeagueFilterComponent,
    LeagueSettingComponent,
    LeagueDataComponent,
    NameStatus,
    UpdateFlagSitedbComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    AngularMaterialModule,
    BrowserAnimationsModule,
    ClipboardModule,
    MatFormFieldModule,
    MatSelectModule,
    MatInputModule,
  ],
  providers: [
    SnackbarService,
    HttpService,
    DeleteTempVariableNumberService,
    UpdateSystemSettingService,
    MatchSettlementLockService,
    MatchParlayLockingService,
    UpdateLogStatusService,
    BlockUIService,
    ConfirmDialogService,
    NavMenuService,
    ControlSyncService,
    SettlementErrorLogService,
    FileService,
    SyncTransIdService,
    UpdateFlagBroadcastService,
    UpdateFlagRebateRetentionService,
    ResultAmendmentService,
    LeagueSettingService,
    UpdateFlagSitedbService,
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
