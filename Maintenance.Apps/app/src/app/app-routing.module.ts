import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ControlSyncServiceComponent } from './control-sync-service/control-sync-service.component';
import { DeleteParlayLockingComponent } from './delete-parlay-locking/delete-parlay-locking.component';
import { DeleteSettlementLockComponent } from './delete-settlement-lock/delete-settlement-lock.component';
import { DeleteTempVariableNumberComponent } from './delete-temp-variable-number/delete-temp-variable-number.component';
import { HomeComponent } from './home/home.component';
import { SyncSettlementLogComponent } from './sync-settlement-log/sync-settlement-log.component';
import { SyncTransidDirectlyComponent } from './sync-transid-directly/sync-transid-directly.component';
import { UpdateLogStatusDetailComponent } from './update-log-status/update-log-status-detail.component';
import { UpdateLogStatusComponent } from './update-log-status/update-log-status.component';
import { UpdateSystemSettingsComponent } from './update-system-settings/update-system-settings.component';

const routes: Routes = [
    // {
    //     path: 'update-log-status',
    //     component: UpdateLogStatusComponent,
    // },
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
                component: SyncSettlementLogComponent
            },
            {
                path: 'sync-transids',
                component: SyncTransidDirectlyComponent
            },
        ]
    }
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule],
})
export class AppRoutingModule { }
