import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DeleteParlayLockingComponent } from './delete-parlay-locking/delete-parlay-locking.component';
import { DeleteSettlementLockComponent } from './delete-settlement-lock/delete-settlement-lock.component';
import { DeleteTempVariableNumberComponent } from './delete-temp-variable-number/delete-temp-variable-number.component';
import { UpdateLogStatusDetailComponent } from './update-log-status-detail/update-log-status-detail.component';
import { UpdateLogStatusComponent } from './update-log-status/update-log-status.component';
import { UpdateSystemSettingsComponent } from './update-system-settings/update-system-settings.component';

const routes: Routes = [
  {
    path: 'update-log',
    component: UpdateLogStatusComponent,
  },
  {
    path: 'update-log/:id',
    component: UpdateLogStatusDetailComponent,
  },
  {
    path: 'delete-temp',
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
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
