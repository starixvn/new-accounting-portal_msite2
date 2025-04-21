import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ControlSyncServiceComponent } from './control-sync-service/control-sync-service.component';
import { DeleteParlayLockingComponent } from './delete-parlay-locking/delete-parlay-locking.component';
import { DeleteSettlementLockComponent } from './delete-settlement-lock/delete-settlement-lock.component';
import { DeleteTempVariableNumberComponent } from './delete-temp-variable-number/delete-temp-variable-number.component';
import { HomeComponent } from './home/home.component';
import { SyncSettlementLogComponent } from './sync-settlement-log/sync-settlement-log.component';
import { SyncTransidDirectlyComponent } from './sync-transid-directly/sync-transid-directly.component';
import { UpdateFlagBroadcastComponent } from './update-flag-broadcast/update-flag-broadcast.component';
import { UpdateFlagRebateRetentionComponent } from './update-flag-rebate-retention/update-flag-rebate-retention.component';
import { UpdateLogStatusDetailComponent } from './update-log-status/update-log-status-detail.component';
import { UpdateSystemSettingsComponent } from './update-system-settings/update-system-settings.component';
import { ResultAmendmentCompoment } from './result-amendment/result-amendment.compoment';
import { LeagueSettingComponent } from './special-league-setting/league-setting.component';
import { UpdateFlagSitedbComponent } from './update-flag-sitedb/update-flag-sitedb.component';

const routes: Routes = [
  {
    path: 'update-log/:id',
    component: UpdateLogStatusDetailComponent,
  },
  {
    path: 'delete-temp-variable',
    component: DeleteTempVariableNumberComponent,
  },
  {
    path: 'delete-parlay-locking',
    component: DeleteParlayLockingComponent,
  },
  {
    path: 'update-system-settings',
    component: UpdateSystemSettingsComponent,
  },
  {
    path: 'delete-settlement-lock',
    component: DeleteSettlementLockComponent,
  },
  {
    path: 'home',
    component: HomeComponent,
  },
  {
    path: 'sync-service',
    children: [
      {
        path: 'on-off-service',
        component: ControlSyncServiceComponent,
      },
      {
        path: 'sync-settlement-log',
        component: SyncSettlementLogComponent,
      },
      {
        path: 'sync-transids',
        component: SyncTransidDirectlyComponent,
      },
    ],
  },
  {
    path: 'on-off-flag',
    children: [
      {
        path: 'update-flag-broadcast',
        component: UpdateFlagBroadcastComponent,
      },
      {
        path: 'update-flag-rebate-retention',
        component: UpdateFlagRebateRetentionComponent,
      },
      {
        path: 'update-flag-sitedb',
        component: UpdateFlagSitedbComponent,
      },
    ],
  },
  {
    path: 'result-amendment',

    children: [
      {
        path: 'update-status',
        component: ResultAmendmentCompoment,
      },
    ],
  },
  {
    path: 'special-league-setting',
    component: LeagueSettingComponent,
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
